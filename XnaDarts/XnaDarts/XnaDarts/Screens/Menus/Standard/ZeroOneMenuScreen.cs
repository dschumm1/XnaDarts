using System;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.ZeroOne;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;

namespace XnaDarts.Screens.Menus.Standard
{
    public class ZeroOneMenuScreen : MenuScreen
    {
        private readonly MenuEntry _me301 = new MenuEntry("301");
        private readonly MenuEntry _me401 = new MenuEntry("401");
        private readonly MenuEntry _me501 = new MenuEntry("501");
        private readonly MenuEntry _me701 = new MenuEntry("701");
        private readonly MenuEntry _me901 = new MenuEntry("901");
        private readonly MenuEntry _meBack = new MenuEntry("Back");

        public ZeroOneMenuScreen() : base("Zero One")
        {
            _me301.OnSelected += me301_OnSelected;
            _me401.OnSelected += me401_OnSelected;
            _me501.OnSelected += me501_OnSelected;
            _me701.OnSelected += me701_OnSelected;
            _me901.OnSelected += me901_OnSelected;

            _meBack.OnSelected += (sender, args) => CancelScreen();

            MenuItems.AddItems(_me301, _me401, _me501, _me701, _me901, _meBack);
        }

        private void me901_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = me901_PlayerSelect;
            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void me901_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneModeScreen((new ZeroOne(players, 901) {MaxRounds = 12})));
        }

        private void me701_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = me701_PlayerSelect;

            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void me701_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneModeScreen(new ZeroOne(players, 701) {MaxRounds = 12}));
        }

        private void me501_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = me501_PlayerSelect;

            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void me501_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneModeScreen(new ZeroOne(players, 501)));
        }

        private void me401_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = me401_PlayerSelect;

            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void me401_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneModeScreen(new ZeroOne(players, 401)));
        }

        private void me301_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = me301_PlayerSelect;

            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void me301_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneModeScreen(new ZeroOne(players, 301)));
        }
    }
}