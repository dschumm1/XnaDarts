using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts.ScreenManagement
{
    public class TimeoutScreen : GameScreen
    {
        private Texture2D _backgroundTexture;
        //private Curve _myCurve;
        private Vector2 _position = new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height)*0.5f;
        private SpriteFont _spriteFont = ScreenManager.Trebuchet64;
        public ContentManager Content;

        public TimeoutScreen(string text, TimeSpan timeout)
        {
            Text = text.ToUpper();

            Timeout = timeout;

            if (Timeout == TimeSpan.Zero)
            {
                Timeout = TimeSpan.MaxValue;
            }

            Position = new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height)*0.5f;
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

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public TimeSpan Timeout { get; set; }
        public Color BackgroundColor { get; set; }
        public event Action OnTimeout;

        public void TimedOut()
        {
            ElapsedTime = 0;

            XnaDartsGame.ScreenManager.RemoveScreen(this);

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
            //_myCurve = Content.Load<Curve>(@"Curves\MyCurve");
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
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture,
                new Rectangle(0, (int) (XnaDartsGame.Viewport.Height*0.5f - _backgroundTexture.Height*0.5f),
                    XnaDartsGame.Viewport.Width, _backgroundTexture.Height - 25), BackgroundColor);

            var origin = _spriteFont.MeasureString(Text)*0.5f;
            var scale = 1.0f; //_myCurve.Evaluate(ElapsedTime*0.001f); //TODO: Fix
            spriteBatch.DrawString(_spriteFont, Text, Position + Vector2.One, Color.Black, 0, origin, scale,
                SpriteEffects.None, 0);
            spriteBatch.DrawString(_spriteFont, Text, Position, Color, 0, origin, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}