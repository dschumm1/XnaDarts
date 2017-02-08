using System;
using Microsoft.Xna.Framework;
using XnaDarts.Gameplay.Modes.ZeroOne;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens.Components;
using XnaDarts.Screens.GameScreens;

namespace XnaDarts.Screens.GameModeScreens
{
    public class ZeroOneModeScreen : BaseModeScreen
    {
        private AwardScreen _awardScreen;
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
            _bustScreen = new TimeoutScreen("Bust", TimeSpan.FromSeconds(3), 0.1f)
            {
                Color = Color.Red
            };

            _bustScreen.OnTimeout += _bustScreenTimeout;

            _bustScreen.LoadContent();

            _awardScreen = new AwardScreen();
            _awardScreen.LoadContent();

            base.LoadContent();
        }

        private void _bustScreenTimeout()
        {
            if (!Mode.IsGameOver)
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
                _handleBust();
            }
            else
            {
                base.HandleEndOfTurn();

                if (Mode.IsLastThrow)
                {
                    _awardScreen.PlayAwards(Mode.CurrentPlayerRound);
                }
            }
        }

        private void _handleBust()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.Bust);
            XnaDartsGame.ScreenManager.AddScreen(_bustScreen);
        }

        public override void StartRound()
        {
            // Stop any award videos that are currently playing
            _awardScreen.Stop();

            base.StartRound();
        }
    }
}