using XnaDarts.Gameplay.Modes.CountUp;
using XnaDarts.Screens.GameModeScreens.Components;
using XnaDarts.Screens.GameScreens;

namespace XnaDarts.Screens.GameModeScreens
{
    public class CountUpModeScreen : BaseModeScreen
    {
        private AwardScreen _awardScreen;

        public CountUpModeScreen(CountUp countUp)
            : base(countUp)
        {
            GuiComponents.Add(new PointsPerDartComponent(countUp));
            GuiComponents.Add(new BigScoreComponent(countUp));
            GuiComponents.Add(new RoundScoresComponent(countUp));
        }

        protected override void HandleEndOfTurn()
        {
            base.HandleEndOfTurn();

            if (Mode.IsLastThrow)
            {
                _awardScreen.PlayAwards(Mode.CurrentPlayerRound);
            }
        }

        public override void LoadContent()
        {
            _awardScreen = new AwardScreen();
            _awardScreen.LoadContent();

            base.LoadContent();
        }

        public override void StartRound()
        {
            //Stop any award videos that are currently playing
            _awardScreen.Stop();

            base.StartRound();
        }
    }
}