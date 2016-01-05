using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class GameModeInformationComponent : IDrawableGameComponent
    {
        private readonly GameMode _mode;

        public GameModeInformationComponent(GameMode mode)
        {
            _mode = mode;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _drawGameModeName(spriteBatch);
            //_drawRoundNumbers(spriteBatch);
            _drawCurrentPlayerName(spriteBatch);
        }

        private void _drawCurrentPlayerName(SpriteBatch spriteBatch)
        {
            var position = new Vector2(20, 200);
            var bigFont = ScreenManager.Trebuchet32;
            TextBlock.DrawShadowed(spriteBatch, bigFont, _mode.CurrentPlayer.Name,
                _mode.GetPlayerColor(_mode.CurrentPlayer),
                position);
            position.Y += bigFont.LineSpacing;
        }

        private void _drawRoundNumbers(SpriteBatch spriteBatch)
        {
            var position = new Vector2(20, 260.0f);
            var smallFont = ScreenManager.Trebuchet22;
            var bigFont = ScreenManager.Trebuchet32;

            var text = "Round:";
            TextBlock.DrawShadowed(spriteBatch, smallFont, text, Color.LightBlue, position);
            position.Y += smallFont.LineSpacing;

            text = (_mode.CurrentRoundIndex + 1) + "/" + _mode.MaxRounds;
            TextBlock.DrawShadowed(spriteBatch, bigFont, text, Color.White, position);
            position.Y += bigFont.LineSpacing;
        }

        private void _drawGameModeName(SpriteBatch spriteBatch)
        {
            var position = Vector2.One*20.0f;
            var smallFont = ScreenManager.Trebuchet22;
            var bigFont = ScreenManager.Trebuchet32;

            var text = "Game Mode:";
            TextBlock.DrawShadowed(spriteBatch, smallFont, text, Color.LightBlue, position);
            position.Y += smallFont.LineSpacing;

            text = _mode.Name;
            TextBlock.DrawShadowed(spriteBatch, bigFont, text, Color.White, position);
            position.Y += bigFont.MeasureString(text).Y;
        }
    }
}