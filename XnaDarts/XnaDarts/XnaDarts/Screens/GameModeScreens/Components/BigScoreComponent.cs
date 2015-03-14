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
        private Texture2D _numbers;
        private readonly GameMode _mode;

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
            var offset = new Vector2(score.Length*NumberWidth*scale, NumberHeight*scale)*0.5f;
            var position = new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height)*0.5f - offset;

            for (var i = 0; i < score.Length; i++)
            {
                if (score[i] == '-')
                {
                    spriteBatch.Draw(_numbers, position, new Rectangle(10*NumberWidth, 0, NumberWidth, NumberHeight),
                        Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    position.X += NumberWidth*scale;
                }
                else
                {
                    var index = int.Parse(score[i].ToString());

                    spriteBatch.Draw(_numbers, position, new Rectangle(index*NumberWidth, 0, NumberWidth, NumberHeight),
                        Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    position.X += NumberWidth*scale;
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            _numbers = content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + "Numbers");
        }
    }
}