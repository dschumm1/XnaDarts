using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameScreens
{
    public class PlayerChangeScreen : TimeoutScreen
    {
        private AnimatedSprite _playerChangeButton;

        public PlayerChangeScreen(string text, TimeSpan timeout)
            : base(text, timeout)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _playerChangeButton = new AnimatedSprite
            {
                Texture = Content.Load<Texture2D>(@"Images\PlayerChangeButton"),
                Frames = 15,
                Reverse = true,
                Fps = 50.0f,
                SourceRectangle = new Rectangle(0, 0, 220, 299)
            };
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            _playerChangeButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            var elapsedWidth = (int) (XnaDartsGame.Viewport.Width*(1f - ElapsedTime/Timeout.TotalMilliseconds));
            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(0, 0, elapsedWidth, 20), Color.White*0.33f);
            _playerChangeButton.Draw(spriteBatch,
                new Vector2(XnaDartsGame.Viewport.Width*0.8f, XnaDartsGame.Viewport.Height*0.5f));
            spriteBatch.End();
        }
    }
}