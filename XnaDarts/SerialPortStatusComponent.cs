using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts
{
    public class SerialPortStatusComponent : DrawableGameComponent
    {
        private AnimatedSprite _serialAnimation;
        private Texture2D _serialDisconnected;
        private SpriteBatch _spriteBatch;

        public SerialPortStatusComponent(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            _serialDisconnected = Game.Content.Load<Texture2D>(@"Images\SerialDisconnected");

            _serialAnimation = new AnimatedSprite
            {
                Frames = 18,
                Texture = Game.Content.Load<Texture2D>(@"Images\Serial"),
                SourceRectangle = new Rectangle(0, 0, 148, 77)
            };

            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        private void drawSerialPortStatus(SpriteBatch spriteBatch)
        {
            var position = new Vector2(XnaDartsGame.Viewport.Width - _serialDisconnected.Width - 20, 20);

            if (!SerialManager.Instance().IsPortOpen)
            {
                spriteBatch.Draw(_serialDisconnected, position, Color.White);
            }
            else
            {
                _serialAnimation.Draw(spriteBatch, position, Vector2.Zero);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            drawSerialPortStatus(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _serialAnimation.Update(gameTime);
        }
    }
}