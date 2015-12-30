using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay.Modes
{
    public class Cricket : GameMode
    {
        public Cricket(int players)
            : base(players)
        {
            for (var i = 20; i >= 15; i--)
            {
                Segments.Add(new CricketSegment(i, Players));
            }

            Segments.Add(new CricketSegment(25, Players));
        }

        protected override void RemoveDart()
        {
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                var dart = CurrentPlayerRound.Darts.Last();

                CurrentPlayerRound.Darts.Remove(dart);

                var segment = Segments.FirstOrDefault(s => s.Segment == dart.Segment);

                if (segment != null)
                {
                    segment.RemoveDart((CricketDart) dart);
                }
            }
        }

        public override int GetScore(Player player)
        {
            return Segments.Sum(segment => segment.GetScore(player));
        }

        public override void RegisterDart(int segment, int multiplier)
        {
            var dart = new CricketDart(CurrentPlayer, segment, multiplier);

            //if the player hit a segment that matters
            var cricketSegment = Segments.FirstOrDefault(s => s.Segment == segment && s.IsOpen);

            if (cricketSegment != null)
            {
                dart.ScoredMarks = multiplier;
                cricketSegment.RegisterDart(dart);
            }

            CurrentPlayerRound.Darts.Add(dart);
        }

        public override bool IsEndOfTurn()
        {
            return base.IsEndOfTurn() ||
                   AllSegmentsAreClosed() ||
                   LeaderOwnsAllOpenSegments();
        }

        public override bool IsGameOver()
        {
            return base.IsGameOver() ||
                   AllSegmentsAreClosed() ||
                   LeaderOwnsAllOpenSegments();
        }

        public bool AllSegmentsAreClosed()
        {
            return Segments.All(s => s.IsOpen == false);
        }

        public bool LeaderOwnsAllOpenSegments()
        {
            var openSegments = Segments.Where(segment => segment.IsOpen);

            return GetLeaders().Any(player => openSegments.All(segment => segment.Owner == player));
        }

        #region Fields and Properties

        /// <summary>
        ///     Key = SegmentIndex, Value = Segment
        /// </summary>
        public List<CricketSegment> Segments = new List<CricketSegment>();

        public override string Name
        {
            get { return "Standard\nCricket"; }
        }

        #endregion
    }
}