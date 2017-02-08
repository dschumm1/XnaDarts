using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts
{
    public class AnimatedSprite
    {
        private int _direction = 1;
        private float _elapsedTime;
        public int CurrentFrame;
        public float Fps = 30.0f;
        public int Frames = 1;
        public bool Reverse = false;
        public Rectangle SourceRectangle = Rectangle.Empty;
        public Texture2D Texture;
        public Color Color = Color.White;

        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTime > 1/Fps*1000.0f)
            {
                _elapsedTime = 0;
                CurrentFrame += _direction;

                if (CurrentFrame > Frames - 1)
                {
                    if (Reverse)
                    {
                        _direction = -1;
                        CurrentFrame = Frames - 1;
                    }
                    else
                    {
                        CurrentFrame = 0;
                    }
                }
                else if (CurrentFrame < 0)
                {
                    CurrentFrame = 0;
                    _direction = 1;
                }
            }
        }

        public void Draw(SpriteBatch batch, Vector2 position, Vector2 offset)
        {
            var frameWidth = Texture.Width/Frames;
            SourceRectangle.X = CurrentFrame*frameWidth;

            batch.Draw(Texture, position - offset, SourceRectangle, Color);
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            var offset = new Vector2(SourceRectangle.Width, SourceRectangle.Height)*0.5f;
            Draw(batch, position, offset);
        }
    }
}