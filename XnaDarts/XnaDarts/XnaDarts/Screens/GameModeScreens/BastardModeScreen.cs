using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.Bastard;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens
{
    public class BastardModeScreen : BaseModeScreen
    {
        private readonly Dartboard _dartboard = new Dartboard();

        public BastardModeScreen(Bastard mode)
            : base(mode)
        {
            foreach (var p in mode.PlayerSegments)
            {
                _dartboard.ColorSegment(p.Key, Mode.GetPlayerColor(p.Value)*0.33f);
            }

            _dartboard.Scale = 0.6f;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _dartboard.LoadContent(Content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            _dartboard.Draw(spriteBatch);
        }
    }
}