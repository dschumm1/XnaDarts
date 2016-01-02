using System;
using XnaDarts.Gameplay.Modes.CountUp;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;

namespace XnaDarts.Screens.Menus.Practice
{
    public class PracticeMenuScreen : MenuScreen
    {
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly MenuEntry _countUp = new MenuEntry("Count Up");
        private readonly MenuEntry _stats = new MenuEntry("Practice History");

        public PracticeMenuScreen()
            : base("Practice")
        {
            _back.OnSelected += (sender, args) => CancelScreen();
            _countUp.OnSelected += CountUp_OnSelected;
            _stats.OnSelected += Stats_OnSelected;

            MenuItems.AddItems(_countUp, _stats, _back);
        }

        private void Stats_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new PracticeHistoryScreen());
        }

        private void CountUp_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(
                new CountUpModeScreen((new CountUp(1) {MaxRounds = 8, IsPractice = true})));
        }
    }
}