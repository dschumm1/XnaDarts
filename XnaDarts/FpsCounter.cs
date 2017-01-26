using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XnaDarts
{
    public class FpsCounter : DrawableGameComponent
    {
        private ContentManager _content;
        private KeyboardState _currentKeyboardState;
        private float _elapsedTime;
        private SpriteFont _font;
        private float _fps;
        private int _frames;
        private KeyboardState _lastKeyBoardState;
        private SpriteBatch _spriteBatch;
        private bool _visible;

        public FpsCounter(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (_content == null)
            {
                _content = new ContentManager(Game.Services, "Content");
            }

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            _font = _content.Load<SpriteFont>(@"Fonts\Trebuchet22");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _frames++;

            if (_visible)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ResolutionHandler.GetTransformationMatrix());
                _spriteBatch.DrawString(_font, "FPS : " + _fps, new Vector2(20, 20) + Vector2.One, Color.Black);
                _spriteBatch.DrawString(_font, "FPS : " + _fps, new Vector2(20, 20), Color.White);
                _spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _lastKeyBoardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            if (_lastKeyBoardState.IsKeyUp(Keys.F5) && _currentKeyboardState.IsKeyDown(Keys.F5))
            {
                _visible = !_visible;
            }

            _elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime >= 1.0f)
            {
                _elapsedTime -= 1.0f;
                _fps = _frames;
                _frames = 0;
            }
        }
    }
}