using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class RoundScoresComponent : IDrawableGameComponent
    {
        private readonly GameMode _mode;

        public RoundScoresComponent(GameMode mode)
        {
            _mode = mode;
        }

        /// <summary>
        ///     Draws the score that is displayed for each round to left of the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(20, XnaDartsGame.Viewport.Height*0.33f);
            var font = ScreenManager.Trebuchet24;

            for (var i = 0; i < _mode.CurrentPlayer.Rounds.Count; i++)
            {
                var round = _mode.CurrentPlayer.Rounds[i];

                var roundScore = _mode.GetScore(round);
                var roundScoreColor = getRoundScoreColor(round);

                if (i == _mode.CurrentRoundIndex)
                {
                    roundScoreColor = Color.Yellow;
                        //Color.Lerp(Color.LightYellow, Color.Yellow, (float) ((Math.Sin(_mode._elapsedTime*1.0f/500f) + 1.0f)/2.0f));
                }
                else if (i > _mode.CurrentRoundIndex)
                {
                    roundScoreColor = Color.White*0.33f;
                }

                var text = "R" + (i + 1) + "." + roundScore;
                TextBlock.DrawShadowed(spriteBatch, font, text, roundScoreColor, position);
                position.Y += font.LineSpacing*0.8f;
            }
        }

        public void LoadContent(ContentManager content)
        {
        }

        private Color getRoundScoreColor(Round round)
        {
            var score = _mode.GetScore(round);

            if (score >= 100)
            {
                return Color.Cyan;
            }
            if (score >= 150)
            {
                return Color.Magenta;
            }

            return Color.White;
        }
    }
}