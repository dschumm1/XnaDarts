using System;

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
}