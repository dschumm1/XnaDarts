using System;
using Microsoft.Xna.Framework;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens.Components;

namespace XnaDarts.Screens.GameModeScreens
{
    public class ZeroOneModeScreen : BaseModeScreen
    {
        private TimeoutScreen _bustScreen;

        public ZeroOneModeScreen(ZeroOne zeroOne) : base(zeroOne)
        {
            GuiComponents.Add(new PointsPerDartComponent(zeroOne));
            GuiComponents.Add(new BigScoreComponent(zeroOne));
            GuiComponents.Add(new RoundScoresComponent(zeroOne));
        }

        private ZeroOne ZeroOne
        {
            get { return (ZeroOne) Mode; }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _bustScreen = new TimeoutScreen("Bust", TimeSpan.FromSeconds(3))
            {
                Color = Color.Red
            };

            _bustScreen.OnTimeout += bustScreenTimeout;

            _bustScreen.LoadContent();
        }

        private void bustScreenTimeout()
        {
            if (!Mode.IsGameOver())
            {
                ShowPlayerChangeScreen();
            }
            else
            {
                HandleGameOver();
            }
        }

        protected override void HandleEndOfTurn()
        {
            if (ZeroOne.IsBust())
            {
                handleBust();
            }
            else
            {
                base.HandleEndOfTurn();
            }
        }

        private void handleBust()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.Bust);
            XnaDartsGame.ScreenManager.AddScreen(_bustScreen);
        }
    }
}