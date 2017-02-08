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
            HighscoreToWin = false;
        }

        #endregion

        public override bool IsEndOfTurn
        {
            get
            {
                return base.IsEndOfTurn ||
                       IsAtZero(CurrentPlayer) ||
                       IsPlayerBustAtCurrentRound(CurrentPlayer);
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

        public override int GetScore(Player player)
        {
            return _getScoreUpToRoundIndex(player, CurrentRoundIndex);
        }

        private int _getScoreUpToRoundIndex(Player player, int roundIndex)
        {
            var score = StartScore;

            for (var i = 0; i <= roundIndex; i++)
            {
                var roundScore = player.Rounds[i].GetScore();
                var scoreAtCurrentIteration = score - roundScore;
                var lastDartInRoundWasADoubleOrTriple = _lastDartInRoundWasADoubleOrTriple(player, i);

                if (_isBustAtScore(scoreAtCurrentIteration, lastDartInRoundWasADoubleOrTriple))
                    continue;

                score -= roundScore;
            }

            return score;
        }

        private static bool _lastDartInRoundWasADoubleOrTriple(Player player, int roundIndex)
        {
            var lastDartOrDefault = player.Rounds[roundIndex].Darts.LastOrDefault();
            var lastDartWasADouble = lastDartOrDefault != null &&
                                     (lastDartOrDefault.Multiplier == 2 || lastDartOrDefault.Multiplier == 3);
            return lastDartWasADouble;
        }

        private bool _isBustAtScore(int score, bool onADoubleOrTriple)
        {
            // If we're below zero we're bust regardless of master out or not
            if (score < 0)
                return true;

            if (IsMasterOut)
            {
                // Bust if we're at 1 in master out
                if (score == 1)
                    return true;

                var isAtZero = score == 0;
                // Bust if we're at zero and last dart was not a double or triple
                if (isAtZero && !onADoubleOrTriple)
                    return true;
            }

            return false;
        }

        public bool IsPlayerBustAtCurrentRound(Player player)
        {
            var lastRoundScore = _getScoreUpToRoundIndex(player, CurrentRoundIndex - 1);
            var finalScore = lastRoundScore - CurrentPlayerRound.GetScore();
            return _isBustAtScore(finalScore, _lastDartInRoundWasADoubleOrTriple(player, CurrentRoundIndex));
        }

        public bool IsAtZero(Player player)
        {
            return GetScore(player) == 0;
        }

        private bool _isLastPlayerAndEndOfTurnAndSomeoneHasWon()
        {
            return IsLastPlayer &&
                   IsEndOfTurn &&
                   Players.Any(p => IsAtZero(p) && !IsPlayerBustAtCurrentRound(p));
        }

        private bool _isBustAndIsLastRound()
        {
            return
                _isBustAtScore(CurrentPlayer.GetScore(), _lastDartInRoundWasADoubleOrTriple(CurrentPlayer, CurrentRoundIndex)) &&
                IsLastRound;
        }

        #region Fields and Properties

        public int StartScore;

        public override string Name
        {
            get { return "01 (" + StartScore + ")"; }
        }

        public bool IsMasterOut { get; set; }

        #endregion
    }
}