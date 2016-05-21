using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts.ScreenManagement
{
    public class DialMenuEntry : MenuEntry
    {
        private const float Spacing = 15.0f;

        public DialMenuEntry(object value, string text)
            : base(text)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override void Draw(SpriteBatch batch, Vector2 position, float transitionAlpha)
        {
            base.Draw(batch, position, transitionAlpha);

            var temp = position + new Vector2(Spacing + Width, 0);

            batch.DrawString(Font, Value.ToString(), temp + Vector2.One, Color.Black*transitionAlpha);
            batch.DrawString(Font, Value.ToString(), temp, Color*transitionAlpha);
        }
    }
}