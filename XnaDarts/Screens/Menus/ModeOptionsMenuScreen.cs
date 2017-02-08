using System;
using System.Collections.Generic;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.ZeroOne;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class ModeOptionsMenuScreen : MenuScreen
    {
        private readonly MenuEntry _meRounds;
        private readonly GameMode _mode;
        private readonly List<int> _rounds;
        private int _roundIndex;

        public ModeOptionsMenuScreen(GameMode mode) : base("Mode Options")
        {
            _mode = mode;

            if (mode.CurrentRoundIndex == 0)
            {
                _meRounds = new MenuEntry("Rounds: " + mode.MaxRounds);

                _rounds = new List<int>();

                var roundChoices = new[] { 2, 3, 8, 15, 20 };

                _rounds.AddRange(roundChoices);
    
                _roundIndex = _rounds.IndexOf(_mode.MaxRounds);
                _meRounds.OnSelected += _meRoundsOnSelected;

                MenuItems.Items.Add(_meRounds);
            }

            var zeroOne = mode as ZeroOne;
            if (zeroOne != null)
            {
                var masterOutOption = new MenuEntry("Master Out: " + (zeroOne.IsMasterOut ? "On" : "Off"));
                masterOutOption.OnSelected +=
                    (sender, args) =>
                    {
                        zeroOne.IsMasterOut = !zeroOne.IsMasterOut;
                        masterOutOption.Text = "Master Out: " + (zeroOne.IsMasterOut ? "On" : "Off");
                    };
                MenuItems.Items.Add(masterOutOption);
            }

            var back = new MenuEntry("Back");
            back.OnSelected += (sender, args) => CancelScreen();

            MenuItems.AddItems(back);
        }

        private void _meRoundsOnSelected(object sender, EventArgs e)
        {
            _roundIndex++;
            _mode.MaxRounds = _rounds[_roundIndex%_rounds.Count];
            _meRounds.Text = "Rounds: " + _mode.MaxRounds;
        }
    }
}