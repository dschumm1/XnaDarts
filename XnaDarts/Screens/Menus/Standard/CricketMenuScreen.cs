using System;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.Cricket;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;

namespace XnaDarts.Screens.Menus.Standard
{
    public class CricketMenuScreen : MenuScreen
    {
        private readonly MenuEntry _meBack = new MenuEntry("Back");
        private readonly MenuEntry _meCutThroat = new MenuEntry("Cut Throat Cricket");
        private readonly MenuEntry _meHidden = new MenuEntry("Hidden Cricket");
        private readonly MenuEntry _mePick = new MenuEntry("Pick Throat Cricket");
        private readonly MenuEntry _meRandom = new MenuEntry("Random Cricket");
        private readonly MenuEntry _meStandard = new MenuEntry("Standard Cricket");

        public CricketMenuScreen() : base("Cricket")
        {
            _meStandard.OnSelected += meStandard_OnSelected;
            _meCutThroat.OnSelected += meCutThroat_OnSelected;
            _meRandom.OnSelected += meRandom_OnSelected;
            _mePick.OnSelected += mePick_OnSelected;
            _meHidden.OnSelected += meHidden_OnSelected;
            _meBack.OnSelected += (sender, args) => CancelScreen();

            MenuItems.AddItems(_meStandard, _meCutThroat, _meBack); //meRandom, mePick, meHidden,
        }

        private void meHidden_OnSelected(object sender, EventArgs e)
        {
        }

        private void mePick_OnSelected(object sender, EventArgs e)
        {
        }

        private void meRandom_OnSelected(object sender, EventArgs e)
        {
        }

        private void meCutThroat_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(2, 4)
            {
                OnPlayerSelect =
                    players => { XnaDartsGame.ScreenManager.AddScreen(new CricketModeScreen((new CutThroatCricket(players)))); }
            };
            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void meStandard_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(2, 4)
            {
                OnPlayerSelect =
                    players => { XnaDartsGame.ScreenManager.AddScreen(new CricketModeScreen((new Cricket(players)))); }
            };
            XnaDartsGame.ScreenManager.AddScreen(screen);
        }
    }
}