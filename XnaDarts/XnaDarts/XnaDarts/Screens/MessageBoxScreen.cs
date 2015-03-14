using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens
{
    public enum MessageBoxButtons
    {
        YesNo,
        YesNoCancel,
        Ok,
        Cancel
    }

    public class MessageBoxScreen : MenuScreen
    {
        private readonly MenuEntry _meCancel = new MenuEntry("Cancel");
        private readonly MenuEntry _meNo = new MenuEntry("No");
        private readonly MenuEntry _meOk = new MenuEntry("Ok");
        private readonly MenuEntry _meYes = new MenuEntry("Yes");

        public MessageBoxScreen(string title, string message, MessageBoxButtons buttons)
            : base(title)
        {
            Message = new TextBlock(message);
            Message.Font = ScreenManager.Trebuchet24;
            StackPanel.Items.Insert(1, Message);

            _meYes.OnSelected += meYes_OnSelected;
            _meNo.OnSelected += meNo_OnSelected;
            _meCancel.OnSelected += meCancel_OnSelected;
            _meOk.OnSelected += meOk_OnSelected;

            _meYes.Font = ScreenManager.Trebuchet24;
            _meNo.Font = ScreenManager.Trebuchet24;
            _meOk.Font = ScreenManager.Trebuchet24;
            _meCancel.Font = ScreenManager.Trebuchet24;

            switch (buttons)
            {
                case MessageBoxButtons.YesNo:
                    MenuItems.AddItems(_meYes, _meNo);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    MenuItems.AddItems(_meYes, _meNo, _meCancel);
                    break;
                case MessageBoxButtons.Ok:
                    MenuItems.AddItems(_meOk);
                    break;
                case MessageBoxButtons.Cancel:
                    MenuItems.AddItems(_meCancel);
                    break;
            }

            MenuPosition = Vector2.One*0.5f -
                           0.5f*new Vector2(StackPanel.Width, StackPanel.Height)/
                           new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height);
        }

        public TextBlock Message { get; set; }
        public event EventHandler OnYes;
        public event EventHandler OnNo;
        public event EventHandler OnCancel;
        public event EventHandler OnOk;

        public void meOk_OnSelected(object sender, EventArgs e)
        {
            if (OnOk != null)
            {
                OnOk(this, null);
            }

            CancelScreen();
        }

        public void meNo_OnSelected(object sender, EventArgs e)
        {
            if (OnNo != null)
            {
                OnNo(this, null);
            }

            CancelScreen();
        }

        public void meCancel_OnSelected(object sender, EventArgs e)
        {
            if (OnCancel != null)
            {
                OnCancel(this, null);
            }

            CancelScreen();
        }

        public void meYes_OnSelected(object sender, EventArgs e)
        {
            if (OnYes != null)
            {
                OnYes(this, null);
            }

            CancelScreen();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            var bgAlpha = 0.66f*TransitionAlpha;
            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle(0, 0, XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height), Color.Black*bgAlpha);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}