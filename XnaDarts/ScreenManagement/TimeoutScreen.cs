using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts.ScreenManagement
{
    public class TimeoutScreen : GameScreen
    {
        private Texture2D _backgroundTexture;
        private SpriteFont _spriteFont = ScreenManager.Trebuchet64;
        public ContentManager Content;
        private bool _hasTimedOut;

        public TimeoutScreen(string text, TimeSpan timeout, float transitionDuration)
        {
            Text = text.ToUpper();

            Timeout = timeout;
            TransitionDuration = transitionDuration;
            TransitionAlpha = 1;

            Position = new Vector2(ResolutionHandler.VWidth, ResolutionHandler.VHeight)*0.5f;
            Color = Color.White;
            BackgroundColor = Color.White*0.5f;
        }

        public string Text { get; set; }

        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
            set { _spriteFont = value; }
        }

        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public TimeSpan Timeout { get; set; }
        public Color BackgroundColor { get; set; }
        public event Action OnTimeout;

        public void TimedOut()
        {
            if (_hasTimedOut)
            {
                return;
            }

            _hasTimedOut = true;
            State = ScreenState.Exiting;

            if (OnTimeout != null)
            {
                OnTimeout();
            }
        }

        public override void LoadContent()
        {
            if (Content == null)
            {
                Content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, "Content");
            }

            _backgroundTexture =
                Content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + "MessageBackground");
        }

        public override void RemoveScreen()
        {
            base.RemoveScreen();
            _resetState();
        }

        /// <summary>
        /// Resetting the state of this screen allows for the same instance to be reused and added again
        /// </summary>
        private void _resetState()
        {
            State = ScreenState.Entering;
            _hasTimedOut = false;
            ElapsedTime = 0;
            TransitionAlpha = 1;
        }

        public override void HandleInput(InputState inputState)
        {
            if (inputState.MenuCancel || inputState.MenuEnter)
            {
                TimedOut();
            }
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            ElapsedTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElapsedTime > Timeout.TotalMilliseconds)
            {
                TimedOut();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                ResolutionHandler.GetTransformationMatrix());
            spriteBatch.Draw(_backgroundTexture,
                new Rectangle(0, (int) (ResolutionHandler.VHeight*0.5f - _backgroundTexture.Height*0.5f),
                    ResolutionHandler.VWidth, _backgroundTexture.Height - 25), BackgroundColor * TransitionAlpha);

            var origin = _spriteFont.MeasureString(Text)*0.5f;
            var scale = 1.0f;
            spriteBatch.DrawString(_spriteFont, Text, Position + Vector2.One, Color.Black * TransitionAlpha, 0, origin, scale,
                SpriteEffects.None, 0);
            spriteBatch.DrawString(_spriteFont, Text, Position, Color * TransitionAlpha, 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}