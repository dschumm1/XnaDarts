using System;
using Microsoft.Xna.Framework;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens.Components;
using XnaDarts.Screens.GameScreens;

namespace XnaDarts.Screens.GameModeScreens
{
    public class ZeroOneModeScreen : BaseModeScreen
    {
        private TimeoutScreen _bustScreen;
        private AwardScreen _awardScreen;

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
            _bustScreen = new TimeoutScreen("Bust", TimeSpan.FromSeconds(3))
            {
                Color = Color.Red
            };

            _bustScreen.OnTimeout += bustScreenTimeout;

            _bustScreen.LoadContent();

            _awardScreen = new AwardScreen();
            _awardScreen.LoadContent();

            base.LoadContent();
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

                if (Mode.IsLastThrow())
                {
                    _awardScreen.PlayAwards(Mode.CurrentPlayerRound);
                }
            }
        }

        private void handleBust()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.Bust);
            XnaDartsGame.ScreenManager.AddScreen(_bustScreen);
        }

        public override void StartRound()
        {
            //Stop any award videos that are currently playing
            _awardScreen.Stop();

            base.StartRound();
        }
    }
}