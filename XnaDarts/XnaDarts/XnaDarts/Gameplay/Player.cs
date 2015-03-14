using System.Collections.Generic;

namespace XnaDarts.Gameplay
{
    public class Player
    {
        public List<Round> Rounds = new List<Round>();

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}