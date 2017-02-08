using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;
using XnaDarts.Screens.GameScreens;

namespace XnaDarts.Screens.Menus
{
    public class PauseMenuScreen : MenuScreen
    {
        private readonly BaseModeScreen _gameModeScreen;
        private readonly MenuEntry _help = new MenuEntry("Help & About");
        private readonly MenuEntry _modeOptions = new MenuEntry("Mode Options");
        private readonly MenuEntry _options = new MenuEntry("Options");
        private readonly MenuEntry _quit = new MenuEntry("Quit");
        private readonly MenuEntry _return = new MenuEntry("Return");
        private readonly MenuEntry _summary = new MenuEntry("Throw Summary");
        private readonly MenuEntry _unthrow = new MenuEntry("Unthrow Last Dart");

        public PauseMenuScreen(BaseModeScreen gameplayScreen)
            : base("Game Paused")
        {
            _gameModeScreen = gameplayScreen;

            _return.OnSelected += (sender, args) => CancelScreen();

            //Check if there is a dart that we can remove
            if (!gameplayScreen.Mode.Players.Any(x => x.Rounds.Any(y => y.Darts.Any())))
                _unthrow.Enabled = false;

            _unthrow.OnSelected += Unthrow_OnSelected;
            _summary.OnSelected += Summary_OnSelected;
            _options.OnSelected += Options_OnSelected;
            _quit.OnSelected += Quit_OnSelected;
            _help.OnSelected += Help_OnSelected;

            _modeOptions.OnSelected += _modeOptions_OnSelected;

            MenuItems.AddItems(_return);

            MenuItems.AddItems(_modeOptions);

            MenuItems.AddItems(_unthrow, _summary, _options, _help, _quit);
        }

        private void _modeOptions_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ModeOptionsMenuScreen(_gameModeScreen.Mode));
        }

        private void Summary_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ThrowSummaryScreen(_gameModeScreen));
        }

        private void Help_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new HelpMenuScreen());
        }

        private void Options_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        private void Unthrow_OnSelected(object sender, EventArgs e)
        {
            var confirm = new MessageBoxScreen("Confirm", "Are you sure you want to remove\nthe last thrown dart?",
                MessageBoxButtons.Yes | MessageBoxButtons.No);
            confirm.OnYes += ConfirmUnthrow_OnAccept;
            XnaDartsGame.ScreenManager.AddScreen(confirm);
        }

        private void ConfirmUnthrow_OnAccept(object sender, EventArgs e)
        {
            _gameModeScreen.Mode.Unthrow();
            CancelScreen();
        }

        private void Quit_OnSelected(object sender, EventArgs e)
        {
            var mbox = new MessageBoxScreen("Quit", "Are you sure you want to quit?",
                MessageBoxButtons.Yes | MessageBoxButtons.No);
            mbox.OnYes += mbox_OnAccept;
            XnaDartsGame.ScreenManager.AddScreen(mbox);
        }

        private void mbox_OnAccept(object sender, EventArgs e)
        {
            _gameModeScreen.RemoveScreen();
            CancelScreen();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                ResolutionHandler.GetTransformationMatrix());
            var bgAlpha = 0.8f;
            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle(0, 0, ResolutionHandler.VWidth, ResolutionHandler.VHeight), Color.Black * bgAlpha);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}