using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens
{
    public class BackgroundScreen : GameScreen
    {
        private Texture2D _background;
        private ContentManager _content;

        public override void LoadContent()
        {
            base.LoadContent();

            if (_content == null)
            {
                _content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, "Content");
            }

            _background = _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\MenuBackground");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(_background, new Rectangle(0, 0, XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height),
                Color.White);
            const string text = "Martin Persson 2015-01-29, www.martinpersson.org";

            var temp = ScreenManager.Arial12.MeasureString(text)*0.5f;
            var offset = new Vector2((int) temp.X, (int) temp.Y);
            var position = new Vector2(XnaDartsGame.Viewport.Width*0.5f,
                XnaDartsGame.Viewport.Height - ScreenManager.Arial12.MeasureString(text).Y);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset, Color.White);
            spriteBatch.End();
        }
    }
}