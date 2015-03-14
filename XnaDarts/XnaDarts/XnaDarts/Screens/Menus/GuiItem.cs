using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public abstract class GuiItem
    {
        public Margin Margin = Margin.Zero;
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public abstract void HandleInput(InputState input);
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha);
    }
}