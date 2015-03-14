using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts.ScreenManagement
{
    /// <summary>
    ///     This is basically a simplified version of the ScreenManager that comes with
    ///     the GameStateManagement sample found here:
    ///     http://create.msdn.com/en-US/education/catalog/sample/game_state_management
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        public static SpriteFont Arial12;
        public static SpriteFont Arial36;
        public static SpriteFont Arial48;
        public static SpriteFont Arial60;
        public static SpriteFont Arial64;
        public static SpriteFont Trebuchet22;
        public static SpriteFont Trebuchet24;
        public static SpriteFont Trebuchet32;
        public static SpriteFont Trebuchet48;
        public static SpriteFont Trebuchet64;
        public static Texture2D BlankTexture;
        public static Texture2D ButtonTexture;
        public static Texture2D ArrowIcon;
        public static Texture2D SelectedButtonTexture;
        public static Texture2D MessageBackground;
        private ContentManager _content;
        private bool _initialized;
        private List<GameScreen> _screensToUpdate;
        private SpriteBatch _spriteBatch;
        private readonly InputState _inputState = new InputState();
        private readonly List<GameScreen> _screens = new List<GameScreen>();

        public ScreenManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _content = Game.Content;

            Arial12 = _content.Load<SpriteFont>(@"Fonts\Arial12");
            Arial36 = _content.Load<SpriteFont>(@"Fonts\Arial36");
            Arial48 = _content.Load<SpriteFont>(@"Fonts\Arial48");
            Arial60 = _content.Load<SpriteFont>(@"Fonts\Arial60");
            Arial64 = _content.Load<SpriteFont>(@"Fonts\Arial64");
            Trebuchet22 = _content.Load<SpriteFont>(@"Fonts\Trebuchet22");
            Trebuchet24 = _content.Load<SpriteFont>(@"Fonts\Trebuchet24");
            Trebuchet32 = _content.Load<SpriteFont>(@"Fonts\Trebuchet32");
            Trebuchet48 = _content.Load<SpriteFont>(@"Fonts\Trebuchet48");
            Trebuchet64 = _content.Load<SpriteFont>(@"Fonts\Trebuchet64");

            BlankTexture = _content.Load<Texture2D>(@"Images\Blank");

            ButtonTexture = _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\Button");
            ArrowIcon = _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\Icons\Arrow");
            SelectedButtonTexture = _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\ButtonSelected");
            MessageBackground =
                _content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + "MessageBackground");

            foreach (var screen in _screens)
            {
                screen.LoadContent();
            }
        }

        public void AddScreen(GameScreen screen)
        {
            _screens.Add(screen);

            if (_initialized)
            {
                screen.LoadContent();
            }
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            _screens.Remove(screen);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _inputState.Update();

            _screensToUpdate = new List<GameScreen>(_screens);

            var isCoveredByOtherScreen = false;

            for (var i = _screensToUpdate.Count - 1; i >= 0; i--)
            {
                var screen = _screensToUpdate[i];

                screen.Update(gameTime, isCoveredByOtherScreen);

                if (!isCoveredByOtherScreen && screen.State != ScreenState.Exiting)
                {
                    if (Game.IsActive)
                    {
                        screen.HandleInput(_inputState);
                    }

                    isCoveredByOtherScreen = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var screensToDraw = new List<GameScreen>(_screens);

            screensToDraw.Reverse();

            while (screensToDraw.Any())
            {
                var screen = screensToDraw[screensToDraw.Count - 1];
                screensToDraw.RemoveAt(screensToDraw.Count - 1);
                screen.Draw(_spriteBatch);
            }
        }
    }
}