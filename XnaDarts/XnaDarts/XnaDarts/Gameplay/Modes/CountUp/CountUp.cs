using System;
using System.Linq;

namespace XnaDarts.Gameplay.Modes.CountUp
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

        public override int GetScore(Player player)
        {
            return player.Rounds.Sum(round => round.GetScore());
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