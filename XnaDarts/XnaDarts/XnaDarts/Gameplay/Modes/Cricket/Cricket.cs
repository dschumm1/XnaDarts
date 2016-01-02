using System;

namespace XnaDarts.Gameplay.Modes.Cricket
{
    public class Cricket : GameMode
    {
        public override bool IsEndOfTurn
        {
            get
            {
                return base.IsEndOfTurn ||
                       _allSegmentsAreClosed() ||
                       _leaderOwnsAllOpenSegments();
            }
        }

        public override bool IsGameOver
        {
            get
            {
                return base.IsGameOver ||
                       _allSegmentsAreClosed() ||
                       _leaderOwnsAllOpenSegments();
            }
        }

        public override int GetScore(Player player)
        {
            return player.GetScore();
        }

        private bool _leaderOwnsAllOpenSegments()
        {
            throw new NotImplementedException();
        }

        private bool _allSegmentsAreClosed()
        {
            throw new NotImplementedException();
        }

        public int GetScoredMarks(Player player, int segment)
        {
            throw new NotImplementedException();
        }

        public bool IsSegmentOpen(int segment)
        {
            throw new NotImplementedException();
        }

        #region Fields and Properties

        private readonly int[] _segments = {20, 19, 18, 17, 16, 15, 25};

        public Cricket(int players)
            : base(players)
        {
        }

        public int[] Segments
        {
            get { return _segments; }
        }

        public override string Name
        {
            get { return "Standard\nCricket"; }
        }

        #endregion
    }
}