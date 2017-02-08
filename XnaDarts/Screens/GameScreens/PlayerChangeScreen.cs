using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameScreens
{
    public class PlayerChangeScreen : TimeoutScreen
    {
        private AnimatedSprite _playerChangeButton;

        public PlayerChangeScreen(string text, TimeSpan timeout, float transitionDuration)
            : base(text, timeout, transitionDuration)
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

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                ResolutionHandler.GetTransformationMatrix());
            var elapsedWidth = (int) (ResolutionHandler.VWidth*(1f - ElapsedTime/Timeout.TotalMilliseconds));
            spriteBatch.Draw(ScreenManager.BlankTexture, new Rectangle(0, 0, elapsedWidth, 20), Color.White*0.33f);
            _playerChangeButton.Color = Color.White*TransitionAlpha;
            _playerChangeButton.Draw(spriteBatch,
                new Vector2(ResolutionHandler.VWidth*0.85f, ResolutionHandler.VHeight*0.5f));
            spriteBatch.End();
        }
    }
}