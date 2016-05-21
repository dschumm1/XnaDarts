using System.Linq;

namespace XnaDarts.Gameplay.Modes.Cricket
{
    public class CutThroatCricket : Cricket
    {
        public CutThroatCricket(int players) : base(players)
        {
            HighscoreToWin = false;
        }

        public override int GetScore(Player player)
        {
            var playerHits = CricketHits.Where(hit => hit.Dart.Player != player);
            return playerHits.Sum(hit => hit.Score);
        }
    }
}