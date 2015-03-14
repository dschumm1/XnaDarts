using Microsoft.Xna.Framework;

namespace XnaDarts.Gameplay
{
    public class Dart
    {
        public Dart(Player player, int segment, int multiplier)
        {
            Player = player;
            Segment = segment;
            Multiplier = multiplier;
        }

        public int GetScore()
        {
            if (Segment == 25)
            {
                // TODO: Allow changing the bull scoring
                return Segment*2; // Regardless of multiplier (single/double bull)
            }

            return Segment*Multiplier;
        }

        public void GetVerbose(out string text, out Color color)
        {
            color = Color.White;
            text = "";

            if (Multiplier == 1)
            {
                text = "Single";
            }
            else if (Multiplier == 2)
            {
                text = "Double";
                color = Color.Yellow;
            }
            else if (Multiplier == 3)
            {
                text = "Triple";
                color = Color.Magenta;
            }

            if (Segment == 0 && Multiplier == 0)
            {
                text = "MISS";
            }
            else if (Segment != 25)
            {
                text += " " + Segment;
            }
            else
            {
                text += " BULL";
                color = Color.Red;
            }
        }

        public override string ToString()
        {
            return "Segment: " + Segment + ", Multiplier: " + Multiplier;
        }

        #region Fields and Properties

        public int Segment;
        public int Multiplier;

        public Player Player;

        #endregion
    }
}