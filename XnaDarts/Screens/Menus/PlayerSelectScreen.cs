using System;
using System.Linq;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class PlayerSelectScreen : MenuScreen
    {
        public delegate void PlayerSelectDelegate(int selectedPlayers);

        public PlayerSelectDelegate OnPlayerSelect;

        public PlayerSelectScreen(int maxPlayers)
            : base("Select Number of Players")
        {
            _initialize(1, maxPlayers);
        }

        public PlayerSelectScreen(int minPlayers, int maxPlayers)
            : base("Select Number of Players")
        {
            _initialize(minPlayers, maxPlayers);
        }

        public PlayerSelectScreen(int[] players)
            : base("Select Number of Players")
        {
            _initialize(players);
        }

        private void _initialize(int[] players)
        {
            foreach (var i in players)
            {
                var entry = new DialMenuEntry(i, "Player");
                entry.OnSelected += Entry_OnSelected;
                MenuItems.Items.Add(entry);
            }

            var back = new MenuEntry("Back");
            back.OnSelected += (sender, args) => CancelScreen();
            MenuItems.Items.Add(back);
        }

        private void _initialize(int minPlayers, int maxPlayers)
        {
            _initialize(Enumerable.Range(minPlayers, maxPlayers - minPlayers + 1).ToArray());
        }

        private void Entry_OnSelected(object sender, EventArgs e)
        {
            var entry = (DialMenuEntry) sender;

            if (OnPlayerSelect != null)
            {
                OnPlayerSelect((int) entry.Value);
            }
        }
    }
}