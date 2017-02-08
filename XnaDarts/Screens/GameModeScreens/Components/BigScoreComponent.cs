using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class BigScoreComponent : IDrawableGameComponent
    {
        private const int NumberHeight = 300;
        private const int NumberWidth = 190;
        private readonly GameMode _mode;
        private Texture2D _numbers;

        public Vector2 Position = new Vector2(0.5f, 0.45f);

        public BigScoreComponent(GameMode mode)
        {
            _mode = mode;
        }

        /// <summary>
        ///     Draws the big score panel in the center of the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            const float scale = 1.0f;
            var score = _mode.GetScore(_mode.CurrentPlayer).ToString();
            var scoreTextOffset = new Vector2(score.Length*NumberWidth*scale, NumberHeight*scale)*0.5f;
            var scorePosition = new Vector2(ResolutionHandler.VWidth, ResolutionHandler.VHeight)*Position -
                                scoreTextOffset;

            for (var i = 0; i < score.Length; i++)
            {
                if (score[i] == '-')
                {
                    spriteBatch.Draw(_numbers, scorePosition,
                        new Rectangle(10*NumberWidth, 0, NumberWidth, NumberHeight),
                        Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    scorePosition.X += NumberWidth*scale;
                }
                else
                {
                    var index = int.Parse(score[i].ToString());

                    spriteBatch.Draw(_numbers, scorePosition,
                        new Rectangle(index*NumberWidth, 0, NumberWidth, NumberHeight),
                        Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    scorePosition.X += NumberWidth*scale;
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            _numbers = content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + "Numbers");
        }
    }
}