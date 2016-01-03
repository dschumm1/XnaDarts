using System;
using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay.Modes.Cricket
{
    public class Cricket : GameMode
    {
        private const int SegmentPrice = 3;
        private readonly List<CricketHit> _cricketHits = new List<CricketHit>();

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
            _cricketHits.RemoveAll(hit => hit.Dart == CurrentPlayerRound.Darts.Last());
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
                            score = dart.GetScore();
                        }
                        else if (costToOpen - dart.Multiplier <= 0)
                        {
                            if (segment != 25)
                            {
                                score = dart.Segment*(dart.Multiplier - costToOpen);
                            }
                            else
                            {
                                score = 50;
                            }
                        }

                        marks = dart.Multiplier;
                    }
                }


                _cricketHits.Add(new CricketHit(dart, marks, score));
            }
        }

        public List<Player> PlayersWhoOwnsSegment(int segment)
        {
            return Players.Where(player => GetScoredMarks(player, segment) >= SegmentPrice).ToList();

            //var segmentHits = _cricketHits.Where(cricketHit => cricketHit.Dart.Segment == segment);

            //var playerHitGroups = segmentHits.GroupBy(hit => hit.Dart.Player);

            //var playersWhoOwnsSegment =
            //    playerHitGroups.Where(hits => hits.Sum(hit => hit.Marks) >= 3).Select(hitGroup => hitGroup.Key).ToList();

            //return playersWhoOwnsSegment;
        }

        public override int GetScore(Player player)
        {
            var playerHits = _cricketHits.Where(hit => hit.Dart.Player == player);
            return playerHits.Sum(hit => hit.Score);
        }

        private bool _leaderOwnsAllOpenSegments()
        {
            return false;
        }

        private bool _allSegmentsAreClosed()
        {
            return !_segments.Where(IsSegmentOpen).Any();
        }

        public int GetScoredMarks(Dart dart)
        {
            var hit = _cricketHits.FirstOrDefault(cricketHit => cricketHit.Dart == dart);
            if (hit == null)
            {
                return 0;
            }
            return hit.Marks;
        }

        public int GetScoredMarks(Player player, int segment)
        {
            return
                _cricketHits.Where(cricketHit => cricketHit.Dart.Player == player && cricketHit.Dart.Segment == segment)
                    .Sum(hit => hit.Marks);
        }

        public bool IsSegmentOpen(int segment)
        {
            var segmentHits = _cricketHits.Where(cricketHit => cricketHit.Dart.Segment == segment);

            var playerHits = segmentHits.GroupBy(hit => hit.Dart.Player);

            var playerHitSum = playerHits.Where(hits => hits.Sum(hit => hit.Marks) >= SegmentPrice);

            var playerHitSumCount = playerHitSum.Count();

            return playerHitSumCount != Players.Count;
        }

        private class CricketHit
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