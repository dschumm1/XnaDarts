using System.Text;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class HelpMenuScreen : MenuScreen
    {
        public HelpMenuScreen()
            : base("Help & About")
        {
            var back = new MenuEntry("Back");
            back.OnSelected += (sender, args) => CancelScreen();

            MenuItems.AddItems(back);

            var sb = new StringBuilder();
            sb.AppendLine("Space - Select/Skip");
            sb.AppendLine("F5 - Toggle FPS");
            sb.AppendLine("F6 - Open dart input");

            StackPanel.Items.Insert(1, new TextBlock(sb.ToString()));
        }
    }
}