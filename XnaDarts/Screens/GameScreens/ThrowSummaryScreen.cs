using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameScreens
{
    public class ThrowSummaryScreen : MenuScreen
    {
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly BaseModeScreen _gameplayScreen;

        public ThrowSummaryScreen(BaseModeScreen gameplayScreen)
            : base("Throw Summary")
        {
            _gameplayScreen = gameplayScreen;
            _back.OnSelected += back_OnSelected;

            MenuItems.AddItems(_back);
        }

        private void back_OnSelected(object sender, EventArgs e)
        {
            CancelScreen();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ResolutionHandler.GetTransformationMatrix());

            const float spacing = 12.0f;
            var players = _gameplayScreen.Mode.Players;
            var width = ResolutionHandler.VWidth/ (float)players.Count * 0.8f;
            var position = new Vector2(ResolutionHandler.VWidth* 0.5f - width * players.Count * 0.5f,
                ResolutionHandler.VHeight* 0.2f);
            var font = ScreenManager.Trebuchet24;

            for (var i = 0; i < players.Count; i++)
            {
                TextBlock.DrawShadowed(spriteBatch, font, players[i].Name,
                    _gameplayScreen.Mode.GetPlayerColor(players[i])*TransitionAlpha,
                    position);
                position.Y += font.MeasureString(players[i].Name).Y + spacing;

                for (var j = 0; j < players[i].Rounds.Count; j++)
                {
                    var text = "";
                    if (i == 0)
                    {
                        text += "R" + (j + 1) + ".";
                    }

                    for (var k = 0; k < players[i].Rounds[j].Darts.Count; k++)
                    {
                        switch (players[i].Rounds[j].Darts[k].Multiplier)
                        {
                            case 2:
                                text += "D";
                                break;
                            case 3:
                                text += "T";
                                break;
                        }

                        text += players[i].Rounds[j].Darts[k].Segment.ToString();

                        if (k != players[i].Rounds[j].Darts.Count - 1)
                        {
                            text += ",";
                        }
                    }

                    TextBlock.DrawShadowed(spriteBatch, font, text, Color.White*TransitionAlpha, position);
                    position.Y += font.LineSpacing + spacing;
                }

                position.X += width + spacing;
                position.Y = ResolutionHandler.VHeight* 0.2f;
            }

            spriteBatch.End();
        }
    }
}