using System;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;

namespace XnaDarts.Screens.Menus.Standard
{
    public class StandardMenuScreen : MenuScreen
    {
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly MenuEntry _countUp = new MenuEntry("Count Up");
        private readonly MenuEntry _cricket = new MenuEntry("Cricket");
        private readonly MenuEntry _zeroOne = new MenuEntry("Zero One");

        public StandardMenuScreen()
            : base("Standard")
        {
            _back.OnSelected += (sender, args) => CancelScreen();
            _zeroOne.OnSelected += ZeroOne_OnSelected;
            _countUp.OnSelected += CountUp_OnSelected;
            _cricket.OnSelected += Cricket_OnSelected;

            MenuItems.AddItems(_zeroOne, _countUp, _cricket, _back);
        }

        private void Cricket_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new CricketMenuScreen());
        }

        private void CountUp_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(4);
            screen.OnPlayerSelect = CountUp_PlayerSelect;
            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void CountUp_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new CountUpModeScreen(new CountUp(players)));
        }

        private void ZeroOne_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new ZeroOneMenuScreen());
        }
    }
}