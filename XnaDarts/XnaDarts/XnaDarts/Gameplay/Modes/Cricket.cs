using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay.Modes
{
    public class CricketSegment
    {
        /// <summary>
        ///     The number of hits a segment costs in order to own it
        /// </summary>
        private const int SegmentMultiplierPrice = 3;

        /// <summary>
        ///     Key = Player, Value = Dart Hits
        /// </summary>
        private readonly Dictionary<Player, List<CricketDart>> _marks = new Dictionary<Player, List<CricketDart>>();

        public readonly int Segment;

        public CricketSegment(int segment, IEnumerable<Player> players)
        {
            Segment = segment;

            //add keys for each player
            foreach (var player in players)
            {
                _marks.Add(player, new List<CricketDart>());
            }
        }

        public bool IsOpen
        {
            get
            {
                var playersWhoHaveTheRequiredMarks = this.playersWhoHaveTheRequiredMarks();
                return playersWhoHaveTheRequiredMarks.Count <= 1;
            }
        }

        public Player Owner
        {
            get
            {
                var playersWhoHaveTheRequiredMarks = this.playersWhoHaveTheRequiredMarks();
                if (playersWhoHaveTheRequiredMarks.Count == 1)
                {
                    return playersWhoHaveTheRequiredMarks.First();
                }
                return null;
            }
        }

        public override string ToString()
        {
            if (Segment == 25)
            {
                return "BULL";
            }
            return Segment.ToString();
        }

        private List<Player> playersWhoHaveTheRequiredMarks()
        {
            return _marks.Where(mark => mark.Value.Sum(dart => dart.ScoredMarks) >= SegmentMultiplierPrice)
                .Select(mark => mark.Key).ToList();
        }

        public void RegisterDart(CricketDart dart)
        {
            _marks[dart.Player].Add(dart);
        }

        public void RemoveDart(CricketDart dart)
        {
            _marks[dart.Player].Remove(dart);
        }

        public int GetScoredMarks(Player player)
        {
            return _marks[player].Sum(dart => dart.ScoredMarks);
        }

        public int GetScore(Player player)
        {
            if (Owner != player)
            {
                return 0;
            }


            return (GetScoredMarks(player) - SegmentMultiplierPrice)*Segment;
        }
    }

    public class CricketDart : Dart
    {
        public CricketDart(Player player, int segment, int multiplier) : base(player, segment, multiplier)
        {
        }

        public int ScoredMarks { get; set; }
    }

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