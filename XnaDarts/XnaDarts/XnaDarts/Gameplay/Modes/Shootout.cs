using System;

namespace XnaDarts.Gameplay.Modes
{
    public class Shootout : GameMode
    {
        public Shootout(int players)
            : base(players)
        {
        }

        public override string Name
        {
            get { return "Shootout"; }
        }

        public override int GetScore(Player player)
        {
            throw new NotImplementedException();
        }
    }
}