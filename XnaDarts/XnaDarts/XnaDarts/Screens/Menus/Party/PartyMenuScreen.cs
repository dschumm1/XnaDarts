using System;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.Bastard;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;

namespace XnaDarts.Screens.Menus.Party
{
    public class PartyMenuScreen : MenuScreen
    {
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly MenuEntry _bastard = new MenuEntry("Bastard Darts");

        public PartyMenuScreen()
            : base("Custom")
        {
            _back.OnSelected += (sender, args) => CancelScreen();
            _bastard.OnSelected += Bastard_OnSelected;
            MenuItems.AddItems(_bastard, _back);
        }

        private void Bastard_OnSelected(object sender, EventArgs e)
        {
            var screen = new PlayerSelectScreen(new[] {2, 4, 5});
            screen.OnPlayerSelect = Bastard_PlayerSelect;
            XnaDartsGame.ScreenManager.AddScreen(screen);
        }

        private void Bastard_PlayerSelect(int players)
        {
            XnaDartsGame.ScreenManager.AddScreen(new BastardModeScreen(new Bastard(players)));
        }
    }
}