using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens
{
    public class BackgroundScreen : GameScreen
    {
        private Texture2D _background;
        private ContentManager _content;

        public override void LoadContent()
        {
            base.LoadContent();

            if (_content == null)
            {
                _content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, "Content");
            }

            _background = _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\MenuBackground");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, ResolutionHandler.GetTransformationMatrix());
            spriteBatch.Draw(_background, new Rectangle(0, 0, ResolutionHandler.VWidth, ResolutionHandler.VHeight),
                Color.White);
            const string text = "Martin Persson 2016-01-25, www.martinpersson.org";

            var textSize = ScreenManager.Arial12.MeasureString(text)*0.5f;
            var offset = new Vector2((int) textSize.X, (int) textSize.Y);
            var position = new Vector2(ResolutionHandler.VWidth * 0.5f, ResolutionHandler.VHeight - ScreenManager.Arial12.MeasureString(text).Y);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset + Vector2.One, Color.Black);
            spriteBatch.DrawString(ScreenManager.Arial12, text, position - offset, Color.White);
            spriteBatch.End();
        }
    }
}