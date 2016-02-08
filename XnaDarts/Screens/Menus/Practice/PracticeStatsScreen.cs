using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus.Practice
{
    public class PracticeHistoryScreen : MenuScreen
    {
        private LineBrush _lineBrush;
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly RecordManager _recordManager;

        public PracticeHistoryScreen() : base("Practice History")
        {
            MenuPosition = new Vector2(0.125f, 0.8f);

            _back.OnSelected += (sender, args) => CancelScreen();

            MenuItems.AddItems(_back);

            _recordManager = RecordManager.Load();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _lineBrush = new LineBrush(2);
            _lineBrush.LoadContent(XnaDartsGame.ScreenManager.GraphicsDevice);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (_recordManager.Records.Count == 0)
            {
                var text = "There are no records saved";
                var offset = ScreenManager.Trebuchet24.MeasureString(text);
                TextBlock.DrawShadowed(spriteBatch, ScreenManager.Trebuchet24, text, Color.White,
                    (new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height) - offset)*0.5f);
            }
            else
            {
                drawGraph(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        private void drawGraph(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle(0, 0, XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height), Color.Black);

            var padding = 60;

            var graphWidth = (int) (XnaDartsGame.Viewport.Width*0.8f) - padding*2;
            var graphHeight = (int) (XnaDartsGame.Viewport.Height*0.6f) - padding*2;
            var graphX = (int) (XnaDartsGame.Viewport.Width*0.1f) + padding;
            var graphY = (int) (XnaDartsGame.Viewport.Width*0.1f) + padding;

            var spacing = graphWidth/Math.Max((_recordManager.Records.Count - 1), 1);

            var minValue = _recordManager.Records.Min(x => x.Score);
            var maxValue = _recordManager.Records.Max(x => x.Score);
            var dv = maxValue - minValue;

            if (dv == 0)
            {
                dv = 1;
            }

            var lastX = graphX;
            var lastY = graphY + graphHeight;

            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle(graphX - padding, graphY - padding, graphWidth + padding*2, graphHeight + padding*2),
                Color.White);

            _lineBrush.Color = Color.Black;

            _lineBrush.Draw(spriteBatch, new Vector2(graphX, graphY), new Vector2(graphX + graphWidth, graphY));
            _lineBrush.Draw(spriteBatch, new Vector2(graphX + graphWidth, graphY),
                new Vector2(graphX + graphWidth, graphY + graphHeight));
            _lineBrush.Draw(spriteBatch, new Vector2(graphX + graphWidth, graphY + graphHeight),
                new Vector2(graphX, graphY + graphHeight));
            _lineBrush.Draw(spriteBatch, new Vector2(graphX, graphY + graphHeight), new Vector2(graphX, graphY));

            _lineBrush.Color = new Color(69, 142, 229);

            for (var i = 0; i < _recordManager.Records.Count; i++)
            {
                var x = graphX + spacing*i;
                var y = graphY + graphHeight - graphHeight*(_recordManager.Records[i].Score - minValue)/dv;

                if (i > 0)
                {
                    _lineBrush.Draw(spriteBatch, new Vector2(lastX, lastY), new Vector2(x, y));
                }

                spriteBatch.DrawString(ScreenManager.Arial12, _recordManager.Records[i].Score.ToString(),
                    new Vector2(x, y), Color.Black);

                var textSize = ScreenManager.Arial12.MeasureString(_recordManager.Records[i].Date.ToShortDateString());

                var offset = textSize*0.5f;

                offset.X = (int) offset.X;
                offset.Y = (int) offset.Y;

                var temp = textSize.X/spacing;

                var n = (int) Math.Round(temp);

                if (Math.Round(temp) < temp)
                {
                    n += 1;
                }

                if (n == 0)
                {
                    n = 1;
                }

                if (i%n == 0)
                {
                    spriteBatch.DrawString(ScreenManager.Arial12, _recordManager.Records[i].Date.ToShortDateString(),
                        new Vector2(x, graphY + graphHeight + padding/2) - offset, Color.Black);
                }

                lastX = x;
                lastY = y;
            }
        }
    }
}