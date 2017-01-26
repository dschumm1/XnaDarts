using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class PointsPerDartComponent : IDrawableGameComponent
    {
        private readonly GameMode _mode;

        public PointsPerDartComponent(GameMode mode)
        {
            _mode = mode;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(ResolutionHandler.VWidth*0.5f, ResolutionHandler.VHeight*0.125f);
            var font = ScreenManager.Trebuchet24;

            float ppd;
            var thrownNumberOfDarts = _mode.CurrentPlayer.Rounds.Sum(r => r.Darts.Count);
            float totalScore = _mode.GetScore(_mode.CurrentPlayer);

            if (thrownNumberOfDarts > 0)
            {
                ppd = totalScore/thrownNumberOfDarts;
            }
            else
            {
                ppd = 0;
            }

            var text = "Points Per Dart: " + ppd.ToString("0.00");
            var offset = font.MeasureString(text)*0.5f;
            spriteBatch.DrawString(font, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(font, text, position - offset, Color.White);

            //Draw points per round
            var ppr = totalScore/(_mode.CurrentRoundIndex + 1);

            position.Y += offset.Y*2.0f + 10.0f;
            text = "Points Per Round: " + ppr.ToString("0.00");
            offset = font.MeasureString(text)*0.5f;
            spriteBatch.DrawString(font, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(font, text, position - offset, Color.White);
        }
    }
}