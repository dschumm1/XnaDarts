using System;
using System.Linq;

namespace XnaDarts.Gameplay.Modes
{
    public class CountUp : GameMode
    {
        private RecordManager _recordManager;
        public bool IsPractice = false;

        public CountUp(int players)
            : base(players)
        {
        }

        public override string Name
        {
            get { return "CountUp"; }
        }

        public override int GetScore(Player p)
        {
            var score = 0; // - Handicap

            score += p.Rounds.Sum(r => GetScore(r));

            return score;
        }

        public void SaveScore(int score)
        {
            var date = DateTime.Now;
            if (_recordManager == null)
            {
                _recordManager = RecordManager.Load();
            }
            _recordManager.Records.Add(new Record(score, date));
        }
    }
}