using System.Linq;

namespace XnaDarts.Gameplay.Modes
{
    public class ZeroOne : GameMode
    {
        #region Constructor

        public ZeroOne(int players, int startScore)
            : base(players)
        {
            StartScore = startScore;
            ScoringDirection = Direction.Asc;
        }

        #endregion

        public override int GetScore(Player player)
        {
            var score = StartScore; // TODO: Maybe add handicap

            foreach (var round in player.Rounds)
            {
                var roundScore = GetScore(round);
                if (score - roundScore >= 0) // Don't count the round if the player went bust
                {
                    score -= roundScore;
                }
            }

            return score;
        }

        public bool IsBust()
        {
            var score = StartScore; // TODO: Maybe add handicap

            for (var i = 0; i < CurrentRoundIndex; i++)
            {
                var roundScore = GetScore(CurrentPlayer.Rounds[i]);
                if (score - roundScore >= 0) // Don't count the round if the player went bust
                {
                    score -= roundScore;
                }
            }

            return score - GetScore(CurrentPlayerRound) < 0;
        }

        public bool HasWon()
        {
            var score = GetScore(CurrentPlayer);
            return score == 0;
        }

        public override bool IsEndOfTurn()
        {
            return base.IsEndOfTurn() ||
                   HasWon() ||
                   IsBust();
        }

        private bool isLastPlayerAndEndOfTurnAndSomeoneHasWon()
        {
            return IsLastPlayer() &&
                   IsEndOfTurn() &&
                   Players.Any(p => GetScore(p) == 0);
        }

        public override bool IsGameOver()
        {
            return base.IsGameOver() ||
                   isLastPlayerAndEndOfTurnAndSomeoneHasWon() ||
                   isBustAndIsLastRound();
        }

        private bool isBustAndIsLastRound()
        {
            return IsBust() && IsLastRound();
        }

        #region Fields and Properties

        public int StartScore;

        public override string Name
        {
            get { return "01 (" + StartScore + ")"; }
        }

        #endregion
    }
}