using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens;
using XnaDarts.Screens.Menus;

namespace XnaDarts
{
    public class XnaDartsGame : Game
    {
        public static GraphicsDeviceManager GraphicsDeviceManager;
        public static ScreenManager ScreenManager;
        public static Options Options;
        public static SoundManager SoundManager;

        public static float ElapsedTime;

        public XnaDartsGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Options = Options.Load();

            _updateResolution();

            SoundManager = new SoundManager(Content);
            ScreenManager = new ScreenManager(this);

            var fpsCounter = new FpsCounter(this);

            Components.Add(ScreenManager);
            Components.Add(fpsCounter);

            Components.Add(new SerialPortStatusComponent(this));

            IsMouseVisible = true;
        }

        public static Viewport Viewport
        {
            get { return ScreenManager.Game.GraphicsDevice.Viewport; }
        }

        private void _updateResolution()
        {
            var resolution = Options.Resolutions[Options.ResolutionIndex];

            GraphicsDeviceManager.PreferredBackBufferWidth = resolution.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = resolution.Height;

            GraphicsDeviceManager.IsFullScreen = Options.FullScreen;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());

            _checkSegmentMap();
        }

        private static void _checkSegmentMap()
        {
            var boundSegments = Options.SegmentMap.Where(x => x.Value != null);
            var count = boundSegments.Count();
            if (count == 0)
            {
                var mb = new MessageBoxScreen("Segment Map Warning",
                    "The segment map does not contain any bindings.\nEnter options to create the segment map.",
                    MessageBoxButtons.Ok);
                ScreenManager.AddScreen(mb);
            }
            else
            {
                var numberOfTotalSegments = 62;
                if (count != numberOfTotalSegments)
                {
                    var mb = new MessageBoxScreen("Segment Map Warning",
                        "It seems like not all segments are bound\n(The segment map contains " + count +
                        " values,\nbut there are 62 segments on a dart board).\nEnter options to create the segment map.",
                        MessageBoxButtons.Ok);
                    ScreenManager.AddScreen(mb);
                }
            }

            foreach (var p1 in boundSegments)
            {
                foreach (var p2 in boundSegments)
                {
                    if (!p1.Key.Equals(p2.Key) && p1.Value.Equals(p2.Value))
                    {
                        Color c;
                        string text1, text2;
                        var dart1 = new Dart(null, p1.Key.X, p1.Key.Y);
                        dart1.GetVerbose(out text1, out c);
                        var dart2 = new Dart(null, p2.Key.X, p2.Key.Y);
                        dart2.GetVerbose(out text2, out c);
                        var mb = new MessageBoxScreen("Segment Map Warning",
                            "The segment: " + text1 + " and " + text2 + "\ncontains the same coordinates: " + p1.Value +
                            "!", MessageBoxButtons.Ok);
                        ScreenManager.AddScreen(mb);
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ElapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void UnloadContent()
        {
            SerialManager.Instance().ClosePort();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}