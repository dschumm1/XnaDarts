using System;

namespace XnaDarts.Gameplay.Modes
{
    public class YamuYamu : GameMode
    {
        public YamuYamu(int players)
            : base(players)
        {
            string[] modes = {"Any Double", "Any Triple", "Bulls-Eye", "One Dart", "Random Segment"};
        }

        public override string Name
        {
            get { return "YamuYamu"; }
        }

        public override int GetScore(Player player)
        {
            throw new NotImplementedException();
        }
    }
}