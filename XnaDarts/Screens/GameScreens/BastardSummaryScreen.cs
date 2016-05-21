using System;
using System.Text;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.Bastard;

namespace XnaDarts.Screens.GameScreens
{
    public class BastardSummaryScreen : MessageBoxScreen
    {
        private readonly Bastard _mode;

        public BastardSummaryScreen(Bastard mode) : base("Throw Summary", "", MessageBoxButtons.Ok)
        {
            _mode = mode;
            OnOk += BastardSummaryScreen_OnOk;
        }

        private void BastardSummaryScreen_OnOk(object sender, EventArgs e)
        {
            CancelScreen();
            //_mode.ShowPlayerChangeScreen();
        }

        public void UpdateText()
        {
            //IEnumerable<IGrouping<Player, BastardHit>> hitsPerPlayer = Mode.BastardHits.Where(hit => hit.Round == Mode.CurrentRoundIndex && hit.ThrownBy == Mode.CurrentPlayer).GroupBy(hit => hit.SegmentOwner);

            var sb = new StringBuilder();

            /*foreach (IGrouping<Player, BastardHit> group in hitsPerPlayer)
            {
                if (group.Key != null) //Segment was owned by someone
                {
                    sb.Append(group.Key.Name + ": ");

                    int temp = 0;

                    foreach (BastardHit hit in group)
                    {
                        if (group.Key == Mode.CurrentPlayer)
                            temp -= hit.Dart.Multiplier;
                        else
                            temp += hit.Dart.Multiplier;
                    }

                    sb.Append(temp.ToString());
                    sb.AppendLine();
                }
                else
                {
                    IEnumerable<IGrouping<int, BastardHit>> temp = group.GroupBy(hit => hit.Dart.Segment);

                    foreach (IGrouping<int, BastardHit> hit in temp)
                    {
                        sb.AppendLine(hit.Key + ": " + hit.Sum(h => h.Dart.Multiplier).ToString());
                    }
                }
            }*/

            Message.Text = sb.ToString();
        }
    }
}