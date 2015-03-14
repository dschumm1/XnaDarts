using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts
{
    public class LineBrush
    {
        private Vector2 _difference;
        private Texture2D _lineTexture;
        private Vector2 _normalizedDifference = Vector2.Zero;
        private float _rotation;
        private Vector2 _scale;
        private float _theta;
        private Vector2 _xVector = new Vector2(1, 0);
        private readonly Vector2 _origin;
        private readonly int _thickness;

        public LineBrush(int thickness)
        {
            _thickness = thickness;
            _origin = new Vector2(0, thickness/2f + 1);
            Color = Color.White;
        }

        public Color Color { get; set; }

        public void LoadContent(GraphicsDevice graphics)
        {
            _lineTexture = new Texture2D(graphics, _thickness + 2, 1, false, SurfaceFormat.Color);

            var count = 2*(_thickness + 2);
            var colorArray = new Color[count];
            colorArray[0] = Color.White;
            colorArray[1] = Color.White;

            for (var i = 2; i < count - 2; i++)
            {
                colorArray[i] = Color.White;
            }

            colorArray[count - 2] = Color.White;
            colorArray[count - 1] = Color.White;

            _lineTexture.SetData(new[] {Color.White, Color.White, Color.White, Color.White});
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint)
        {
            Vector2.Subtract(ref endPoint, ref startPoint, out _difference);
            calculateRotation(ref _difference);
            calculateScale(ref _difference);

            //Note: Scale is used to create the thickness
            spriteBatch.Draw(_lineTexture, startPoint, null, Color, _rotation, _origin, _scale, SpriteEffects.None, 0);
        }

        private void calculateRotation(ref Vector2 difference)
        {
            Vector2.Normalize(ref difference, out _normalizedDifference);
            Vector2.Dot(ref _xVector, ref _normalizedDifference, out _theta);

            _theta = (float) Math.Acos(_theta);
            if (difference.Y < 0)
            {
                _theta = -_theta;
            }
            _rotation = _theta;
        }

        private void calculateScale(ref Vector2 difference)
        {
            var desiredLength = difference.Length();
            _scale.X = desiredLength/_lineTexture.Width;
            _scale.Y = 1;
        }
    }
}