namespace XnaDarts.Gameplay.Modes
{
    public class CricketDart : Dart
    {
        public CricketDart(Player player, int segment, int multiplier) : base(player, segment, multiplier)
        {
        }

        public int ScoredMarks { get; set; }
    }
}