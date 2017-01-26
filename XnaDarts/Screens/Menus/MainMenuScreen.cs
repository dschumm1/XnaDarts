using System;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus.Party;
using XnaDarts.Screens.Menus.Practice;
using XnaDarts.Screens.Menus.Standard;

namespace XnaDarts.Screens.Menus
{
    public class MainMenuScreen : MenuScreen
    {
        private readonly MenuEntry _help = new MenuEntry("Help & About");
        private readonly MenuEntry _meOptions = new MenuEntry("Options");
        private readonly MenuEntry _party = new MenuEntry("Custom");
        private readonly MenuEntry _practice = new MenuEntry("Practice");
        private readonly MenuEntry _quit = new MenuEntry("Quit");
        private readonly MenuEntry _standard = new MenuEntry("Standard");

        public MainMenuScreen()
            : base("Main Menu")
        {
            _meOptions.OnSelected += meOptions_OnSelected;
            _quit.OnSelected += Quit_OnSelected;
            _standard.OnSelected += Standard_OnSelected;
            _practice.OnSelected += Practice_OnSelected;
            _party.OnSelected += Party_OnSelected;
            _help.OnSelected += Help_OnSelected;

            //Practice.Enabled = false;
            //Removed practice 2012-12-21
            MenuItems.AddItems(_standard, _party, _practice, _meOptions, _help, _quit);
        }

        private void Quit_OnSelected(object sender, EventArgs e)
        {
            CancelScreen();
        }

        private void Help_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new HelpMenuScreen());
        }

        private void Party_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new PartyMenuScreen());
        }

        private void Practice_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new PracticeMenuScreen());
        }

        private void Standard_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new StandardMenuScreen());
        }

        private void meOptions_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        private void confirmQuit(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.Game.Exit();
        }

        public override void CancelScreen()
        {
            var confirm = new MessageBoxScreen("Quit", "Are you sure you want to quit?", MessageBoxButtons.Yes | MessageBoxButtons.No);
            confirm.OnYes += confirmQuit;
            XnaDartsGame.ScreenManager.AddScreen(confirm);
        }
    }
}