using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay
{
    public class Player
    {
        public List<Round> Rounds = new List<Round>();
        public int Handicap;

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public int GetScore()
        {
            return Rounds.Sum(round => round.GetScore());
        }
    }
}