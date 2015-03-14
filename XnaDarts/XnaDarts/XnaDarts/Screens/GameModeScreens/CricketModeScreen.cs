using XnaDarts.Gameplay.Modes;
using XnaDarts.Screens.GameModeScreens.Components;

namespace XnaDarts.Screens.GameModeScreens
{
    public class CricketModeScreen : BaseModeScreen
    {
        public CricketModeScreen(Cricket cricket)
            : base(cricket)
        {
            GuiComponents.Add(new CricketMarksComponent(cricket));
            GuiComponents.Add(new CricketRoundMarksComponent(cricket));
        }
    }
}