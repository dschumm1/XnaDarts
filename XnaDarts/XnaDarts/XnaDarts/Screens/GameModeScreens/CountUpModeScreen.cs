using XnaDarts.Gameplay.Modes;
using XnaDarts.Screens.GameModeScreens.Components;

namespace XnaDarts.Screens.GameModeScreens
{
    public class CountUpModeScreen : BaseModeScreen
    {
        public CountUpModeScreen(CountUp countUp)
            : base(countUp)
        {
            GuiComponents.Add(new PointsPerDartComponent(countUp));
            GuiComponents.Add(new BigScoreComponent(countUp));
            GuiComponents.Add(new RoundScoresComponent(countUp));
        }
    }
}