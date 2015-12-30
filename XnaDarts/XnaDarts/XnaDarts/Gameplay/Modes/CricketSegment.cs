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
                var playersWhoHaveTheRequiredMarks = this._playersWhoHaveTheRequiredMarks();
                return playersWhoHaveTheRequiredMarks.Count <= 1;
            }
        }

        private Player _owner;

        public Player Owner
        {
            get
            {
                return _getOwner();
            }
        }

        private Player _getOwner()
        {
            _checkIfCurrentOwnerStillHasRequiredMarks();
            _assignOwner();
            return _owner;
        }

        private void _assignOwner()
        {
            var playersWhoHaveTheRequiredMarks = this._playersWhoHaveTheRequiredMarks();
            if (playersWhoHaveTheRequiredMarks.Count == 1)
            {
                _owner = playersWhoHaveTheRequiredMarks.First();
            }
        }

        private void _checkIfCurrentOwnerStillHasRequiredMarks()
        {
            // remove the owner if he no longer has the required marks
            if (_owner != null && _marks[_owner].Sum(dart => dart.ScoredMarks) < SegmentMultiplierPrice)
            {
                _owner = null;
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

        private List<Player> _playersWhoHaveTheRequiredMarks()
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
}