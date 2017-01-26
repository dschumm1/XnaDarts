using System.Linq;

namespace XnaDarts.Gameplay.Modes.ZeroOne
{
    public class ZeroOne : GameMode
    {
        private readonly bool _isMasterOut = true;
        private bool _isMasterIn = false;

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

        public int BustLimit
        {
            get { return _isMasterOut ? 1 : 0; }
        }

        public override int GetScore(Player player)
        {
            var score = StartScore;

            for (var i = 0; i < CurrentRoundIndex; i++)
            {
                var roundScore = player.Rounds[i].GetScore();
                var lastOrDefault = player.Rounds[i].Darts.LastOrDefault();
                if (score - roundScore < BustLimit || (score - roundScore == 0 && (lastOrDefault != null && lastOrDefault.Multiplier == 1))) // Don't count if the player went bust
                {
                    continue;
                }
                score -= roundScore;
            }

            return score - player.Rounds[CurrentRoundIndex].GetScore();
        }

        public bool IsBust()
        {
            return GetScore(CurrentPlayer) < BustLimit;
        }

        public bool HasWon()
        {
            if (_isMasterOut)
            {
                return GetScore(CurrentPlayer) == 0 && _lastDartWasDouble();
            }
            return GetScore(CurrentPlayer) == 0;
        }

        private bool _lastDartWasDouble()
        {
            var lastRound = CurrentPlayer.Rounds.LastOrDefault();
            if (lastRound == null)
            {
                return false;
            }
            var lastDart = lastRound.Darts.LastOrDefault();
            if (lastDart == null)
            {
                return false;
            }
            return (lastDart.Multiplier > 1 || lastDart.Segment == 25);
        }

        private bool _isLastPlayerAndEndOfTurnAndSomeoneHasWon()
        {
            return IsLastPlayer &&
                   IsEndOfTurn &&
                   Players.Any(p => GetScore(p) == 0);
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