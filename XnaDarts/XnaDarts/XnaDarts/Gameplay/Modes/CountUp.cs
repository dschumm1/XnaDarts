using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace XnaDarts.Gameplay.Modes
{
    [Serializable]
    public struct Record
    {
        public DateTime Date;
        public int Score;

        public Record(int score, DateTime date)
        {
            Score = score;
            Date = date;
        }

        public override string ToString()
        {
            return Score + "          " + Date;
        }
    }

    [Serializable]
    public class RecordManager
    {
        public const string FileName = "records.sd";
        public List<Record> Records = new List<Record>();

        public static RecordManager Load()
        {
            var rm = new RecordManager();

            if (File.Exists(FileName))
            {
                var bf = new BinaryFormatter();
                var fs = new FileStream(FileName, FileMode.Open);
                rm.Records = (List<Record>) bf.Deserialize(fs);
                fs.Close();
            }

            return rm;
        }

        public void Save()
        {
            var bf = new BinaryFormatter();
            var fs = new FileStream(FileName, FileMode.Create);
            bf.Serialize(fs, Records);
            fs.Close();
        }
    }

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