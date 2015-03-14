using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Screens.Menus;

namespace XnaDarts.ScreenManagement
{
    public enum MenuEntryIcon
    {
        None,
        Arrow,
        PlusMinus,
        Check,
        Cancel
    }

    public class MenuEntry : TextBlock
    {
        public bool Enabled = true;
        public MenuEntryIcon Icon = MenuEntryIcon.None;

        public MenuEntry(string text) : base(text)
        {
        }

        public event EventHandler OnSelected;
        public event EventHandler OnCanceled;
        public event EventHandler OnMenuLeft;
        public event EventHandler OnMenuRight;

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            handleKeyboardInput(input);
        }

        public void MenuLeft()
        {
            if (OnMenuLeft != null && Enabled)
            {
                OnMenuLeft(this, null);
            }
        }

        public void MenuRight()
        {
            if (OnMenuRight != null && Enabled)
            {
                OnMenuRight(this, null);
            }
        }

        public void Select()
        {
            if (OnSelected != null && Enabled)
            {
                XnaDartsGame.SoundManager.PlaySound(SoundCue.MenuEnter);
                OnSelected(this, null);
            }
        }

        public void Cancel()
        {
            if (OnCanceled != null && Enabled)
            {
                XnaDartsGame.SoundManager.PlaySound(SoundCue.MenuBack);
                OnCanceled(this, null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, float transitionAlpha)
        {
            var color = Color;
            if (!Enabled)
            {
                Color = color*0.33f;
            }
            base.Draw(spriteBatch, position, transitionAlpha);
            Color = color;
        }

        private void handleKeyboardInput(InputState input)
        {
            if (input.MenuLeft)
            {
                MenuLeft();
            }

            if (input.MenuRight)
            {
                MenuRight();
            }

            if (input.MenuEnter)
            {
                Select();
            }

            if (input.MenuBack)
            {
                Cancel();
            }
        }
    }
}