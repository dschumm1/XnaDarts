using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public interface IDrawableGameComponent
    {
        void LoadContent(ContentManager content);
        void Draw(SpriteBatch spriteBatch);
    }

    public class PlayerPanelsComponent : IDrawableGameComponent
    {
        private const int PlayerPanelMaxWidth = 400;
        private Texture2D _crown;
        private Texture2D _glow;
        private float _glowAlpha;
        private Texture2D _playerNameBackground;
        private readonly GameMode _mode;

        public PlayerPanelsComponent(GameMode mode)
        {
            _mode = mode;
        }

        public void LoadContent(ContentManager content)
        {
            _crown = content.Load<Texture2D>(@"Images\Crown");
            _glow = content.Load<Texture2D>(@"Images\Glow");
            _playerNameBackground = content.Load<Texture2D>(@"Images\PlayerNameBackground");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _glowAlpha = 1.0f; //(float) (Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1.0f)/2.0f;

            var bigFont = ScreenManager.Trebuchet32;
            var scoreFont = ScreenManager.Trebuchet48;

            var playerPanelWidth = ResolutionHandler.VWidth/_mode.Players.Count;

            if (playerPanelWidth > PlayerPanelMaxWidth)
            {
                playerPanelWidth = PlayerPanelMaxWidth;
            }

            var position =
                new Vector2(ResolutionHandler.VWidth*0.5f - _mode.Players.Count/2.0f*playerPanelWidth,
                    ResolutionHandler.VHeight*0.8f);
            var leaders = _mode.GetLeaders();
            for (var i = 0; i < _mode.Players.Count; i++)
            {
                var text = _mode.Players[i].Name;
                var nameFont = bigFont;
                var nameSize = nameFont.MeasureString(text);

                var background = Color.White*0.33f;
                var foreground = Color.White;
                var shadow = Color.Black;

                var scoreBackground = _mode.GetPlayerColor(_mode.Players[i]);
                var namePanelRectangle = new Rectangle((int) position.X, (int) position.Y, playerPanelWidth,
                    (int) nameSize.Y);

                var score = _mode.GetScore(_mode.Players[i]).ToString();
                var scoreSize = scoreFont.MeasureString(score);

                var y = (int) (position.Y + nameSize.Y);
                var height = ResolutionHandler.VHeight - y;
                var scoreRectangle = new Rectangle((int) position.X, y, playerPanelWidth, height);

                if (_mode.CurrentPlayerIndex == i)
                {
                    background = Color.White;
                    shadow = Color.White;
                    foreground = _mode.GetPlayerColor(_mode.Players[i]);

                    //Draw glow
                    spriteBatch.Draw(_glow, new Vector2(scoreRectangle.Center.X, scoreRectangle.Center.Y), null,
                        _mode.GetPlayerColor(_mode.Players[i])*_glowAlpha, 0,
                        new Vector2(_glow.Width, _glow.Height)*0.5f, 3.0f,
                        SpriteEffects.None, 0);
                }
                else
                {
                    scoreBackground *= 0.33f;
                }

                //Draw player name panel
                spriteBatch.Draw(_playerNameBackground, namePanelRectangle, background);

                //Draw player name
                var center = new Vector2(playerPanelWidth, nameSize.Y)*0.5f;
                var offset = nameSize*0.5f;
                spriteBatch.DrawString(nameFont, text, position + center - offset + Vector2.One, shadow);
                spriteBatch.DrawString(nameFont, text, position + center - offset, foreground);

                //Draw score background rectangle
                spriteBatch.Draw(ScreenManager.BlankTexture, scoreRectangle, scoreBackground);

                center = new Vector2(playerPanelWidth, height)*0.5f;
                offset = scoreSize*0.5f;

                //Draw score
                var scorePos = new Vector2(position.X, y) + center - offset;
                TextBlock.DrawShadowed(spriteBatch, scoreFont, score, Color.White, scorePos);

                //Draw crown
                if (leaders.Contains(_mode.Players[i]))
                {
                    spriteBatch.Draw(_crown, position + new Vector2(0, nameSize.Y + 10), null, Color.White, 0,
                        Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                }

                position.X += playerPanelWidth;
            }
        }
    }
}