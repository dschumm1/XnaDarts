using System;
using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay.Modes.Cricket
{
    public class Cricket : GameMode
    {
        private const int SegmentPrice = 3;
        protected readonly List<CricketHit> CricketHits = new List<CricketHit>();

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

        protected override void _removeDart()
        {
            CricketHits.RemoveAll(hit => hit.Dart == CurrentPlayerRound.Darts.Last());
            base._removeDart();
        }

        public override void RegisterDart(int segment, int multiplier)
        {
            base.RegisterDart(segment, multiplier);

            if (_segments.Any(x => x == segment))
            {
                var dart = CurrentPlayerRound.Darts.Last();

                var score = 0;
                var marks = 0;

                if (IsSegmentOpen(segment))
                {
                    var playersWhoOwnsSegment = PlayersWhoOwnsSegment(segment);
                    var currentPlayerOwnsSegment = playersWhoOwnsSegment.Contains(CurrentPlayer);
                    var isLastPlayerToOwnSegment = playersWhoOwnsSegment.Count == Players.Count - 1;

                    if (!currentPlayerOwnsSegment && isLastPlayerToOwnSegment)
                    {
                        var hitMarks = GetScoredMarks(CurrentPlayer, segment);
                        var costToOpen = SegmentPrice - hitMarks;

                        marks = Math.Min(costToOpen, dart.Multiplier);
                    }
                    else
                    {
                        var hitMarks = GetScoredMarks(CurrentPlayer, segment);
                        var costToOpen = SegmentPrice - hitMarks;

                        if (currentPlayerOwnsSegment)
                        {
                            score = dart.Segment*dart.Multiplier;
                        }
                        else if (costToOpen - dart.Multiplier < 0)
                        {
                            score = dart.Segment*(dart.Multiplier - costToOpen);
                        }

                        marks = dart.Multiplier;
                    }
                }


                CricketHits.Add(new CricketHit(dart, marks, score));
            }
        }

        public List<Player> PlayersWhoOwnsSegment(int segment)
        {
            return Players.Where(player => GetScoredMarks(player, segment) >= SegmentPrice).ToList();
        }

        public override int GetScore(Player player)
        {
            var playerHits = CricketHits.Where(hit => hit.Dart.Player == player);
            return playerHits.Sum(hit => hit.Score);
        }

        private bool _leaderOwnsAllOpenSegments()
        {
            if (Players.Count == 2)
            {
                var leaders = GetLeaders();
                if (leaders.Count == 1)
                {
                    var openSegments = _segments.Where(IsSegmentOpen);
                    return openSegments.All(segment => PlayersWhoOwnsSegment(segment).Contains(leaders.First()));
                }
            }

            return false;
        }

        private bool _allSegmentsAreClosed()
        {
            return !_segments.Where(IsSegmentOpen).Any();
        }

        public int GetScoredMarks(Dart dart)
        {
            var hit = CricketHits.FirstOrDefault(cricketHit => cricketHit.Dart == dart);
            if (hit == null)
            {
                return 0;
            }
            return hit.Marks;
        }

        public int GetScoredMarks(Player player, int segment)
        {
            return
                CricketHits.Where(cricketHit => cricketHit.Dart.Player == player && cricketHit.Dart.Segment == segment)
                    .Sum(hit => hit.Marks);
        }

        public bool IsSegmentOpen(int segment)
        {
            // hits in a segment which is part of the cricket 15-20 and bull
            var segmentHits = CricketHits.Where(cricketHit => cricketHit.Dart.Segment == segment);

            // group each hit by player
            var playerHits = segmentHits.GroupBy(hit => hit.Dart.Player);

            // sum all of the marks hit for each segment and check if its above or equal to the price of that segment
            var playerHitSum = playerHits.Where(hits => hits.Sum(hit => hit.Marks) >= SegmentPrice);

            // get the number of players who have reached the price
            var playerHitSumCount = playerHitSum.Count();

            // the segment is open as long as not all players have hit the price
            return playerHitSumCount != Players.Count;
        }

        protected class CricketHit
        {
            public CricketHit(Dart dart, int marks, int score)
            {
                Dart = dart;
                Marks = marks;
                Score = score;
            }

            public Dart Dart { get; private set; }
            public int Marks { get; private set; }
            public int Score { get; private set; }
        }

        #region Fields and Properties

        private readonly int[] _segments = {20, 19, 18, 17, 16, 15, 25};

        public Cricket(int players)
            : base(players)
        {
            MaxRounds = 15;
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