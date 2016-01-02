using System.Linq;

namespace XnaDarts.Gameplay.Modes.ZeroOne
{
    public class ZeroOne : GameMode
    {
        #region Constructor

        public ZeroOne(int players, int startScore)
            : base(players)
        {
            StartScore = startScore;
        }

        #endregion

        public override bool IsEndOfTurn
        {
            get
            {
                return base.IsEndOfTurn ||
                       HasWon() ||
                       IsBust();
            }
        }

        public override bool IsGameOver
        {
            get
            {
                return base.IsGameOver ||
                       _isLastPlayerAndEndOfTurnAndSomeoneHasWon() ||
                       _isBustAndIsLastRound();
            }
        }

        public bool IsBust()
        {
            var score = StartScore; // TODO: Maybe add handicap

            for (var i = 0; i < CurrentRoundIndex; i++)
            {
                var roundScore = CurrentPlayer.Rounds[i].GetScore();
                if (score - roundScore >= 0) // Don't count the round if the player went bust
                {
                    score -= roundScore;
                }
            }

            return score - CurrentPlayerRound.GetScore() < 0;
        }

        public bool HasWon()
        {
            return GetScore(CurrentPlayer) == 0;
        }

        private bool _isLastPlayerAndEndOfTurnAndSomeoneHasWon()
        {
            return IsLastPlayer &&
                   IsEndOfTurn &&
                   Players.Any(p => GetScore(p) == 0);
        }

        public override int GetScore(Player player)
        {
            return StartScore - player.GetScore();
        }

        private bool _isBustAndIsLastRound()
        {
            return IsBust() && IsLastRound;
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