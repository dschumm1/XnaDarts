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
            spriteBatch.Draw(_background, new Rectangle(0, 0, ResolutionHandler.VWidth, ResolutionHandler.VHeight), Color.White);
            spriteBatch.End();
        }
    }
}