using System.Collections.Generic;
using System.Linq;

namespace XnaDarts.Gameplay
{
    public class Round
    {
        public List<Dart> Darts = new List<Dart>();

        public int GetScore()
        {
            return Darts.Sum(dart => dart.GetScore());
        }
    }
}