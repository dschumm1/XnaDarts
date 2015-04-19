using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.GameModeScreens.Components;
using XnaDarts.Screens.GameScreens;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens
{
    public abstract class BaseModeScreen : GameScreen
    {
        #region Constructor

        protected BaseModeScreen(GameMode mode)
        {
            Mode = mode;

            // Default GUI Components
            GuiComponents = new List<IDrawableGameComponent>();
            GuiComponents.AddRange(new IDrawableGameComponent[]
            {
                new PlayerPanelsComponent(Mode),
                new GameModeInformationComponent(Mode),
                new DartScoreComponent(Mode),
                new BigScoreComponent(Mode)
            });
        }

        #endregion

        private void startGame()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.GameStart);
            StartRound();
        }

        /// <summary>
        ///     This method is fired by the serial manager when a dart hits the dartboard
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="multiplier"></param>
        private void registerDart(int segment, int multiplier)
        {
            if (Mode.IsEndOfTurn())
            {
                return;
            }

            //Exit screens in case they are still in view
            _newRoundTimeoutScreen.RemoveScreen();
            _throwDartsScreen.RemoveScreen();

            Mode.RegisterDart(segment, multiplier);

            playDartHitSound(segment, multiplier);

            if (Mode.IsEndOfTurn())
            {
                HandleEndOfTurn();
            }
        }

        private void playerChange()
        {
            Mode.NextPlayer();

            if (Mode.IsFirstPlayer())
            {
                StartRound();
            }
            else if (Mode.IsFirstThrow())
            {
                startTurn();
            }
        }

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Rectangle(0, 0, XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height),
                Mode.GetPlayerColor(Mode.CurrentPlayer));

            foreach (var drawableGameComponent in GuiComponents)
            {
                drawableGameComponent.Draw(spriteBatch);
            }

            spriteBatch.End();

            if (_showDartboard)
            {
                _dartboard.Draw(spriteBatch);
            }
        }

        #endregion

        #region Fields and Properties

        private Texture2D _background;
        private bool _showDartboard;
        protected ContentManager Content;
        public List<IDrawableGameComponent> GuiComponents;
        public GameMode Mode;

        #region Screens

        private TimeoutScreen _newRoundTimeoutScreen;
        private PlayerChangeScreen _playerChangeScreen;
        private TimeoutScreen _throwDartsScreen;
        private Dartboard _dartboard;

        #endregion

        #endregion

        #region SoundHelper

        private void playNewRoundSound()
        {
            if (Mode.IsLastRound())
            {
                XnaDartsGame.SoundManager.PlaySound(SoundCue.LastRound);
            }
            else
            {
                XnaDartsGame.SoundManager.PlaySound(SoundCue.NewRound);
            }
        }

        private static void playDartHitSound(int segment, int multiplier)
        {
            if (segment == 25)
            {
                if (multiplier == 2)
                {
                    XnaDartsGame.SoundManager.PlaySound(SoundCue.DoubleBull);
                }
                else
                {
                    XnaDartsGame.SoundManager.PlaySound(SoundCue.SingleBull);
                }
            }
            else
            {
                if (multiplier == 2)
                {
                    XnaDartsGame.SoundManager.PlaySound(SoundCue.Double);
                }
                else if (multiplier == 3)
                {
                    XnaDartsGame.SoundManager.PlaySound(SoundCue.Triple);
                }
                else
                {
                    XnaDartsGame.SoundManager.PlaySound(SoundCue.Single);
                }
            }
        }

        #endregion

        #region Load/Unload Content

        public override void LoadContent()
        {
            if (Content == null)
            {
                Content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, @"Content");
            }

            _background = Content.Load<Texture2D>(@"Images\Backgrounds\AbstractBackground"); // XnaDartsGame.Options.Theme

            SerialManager.Instance().OnDartRegistered = registerDart;
            SerialManager.Instance().OnDartHit = null;

            _dartboard = new Dartboard();
            _dartboard.LoadContent(Content);
            _dartboard.OnSegmentClicked += registerDart;
            _dartboard.Scale = 0.5f;

            _playerChangeScreen = new PlayerChangeScreen("Player Change",
                TimeSpan.FromSeconds(XnaDartsGame.Options.PlayerChangeTimeout));
            _playerChangeScreen.LoadContent();
            _playerChangeScreen.OnTimeout += playerChange;

            _throwDartsScreen = new TimeoutScreen(Mode.CurrentPlayer.Name + " throw darts!", TimeSpan.FromSeconds(3));
            _newRoundTimeoutScreen = new TimeoutScreen("Round 1", TimeSpan.FromSeconds(3));
            _newRoundTimeoutScreen.OnTimeout += startTurn;

            foreach (var drawableGameComponent in GuiComponents)
            {
                drawableGameComponent.LoadContent(Content);
            }

            startGame();
        }

        public override void UnloadContent()
        {
            SerialManager.Instance().OnDartRegistered = null;
            Content.Unload();
        }

        #endregion

        #region Handle Input

        private void pause()
        {
            var pause = new PauseMenuScreen(this);
            XnaDartsGame.ScreenManager.AddScreen(pause);
        }

        /// <summary>
        ///     This method is called by pressing the skip button, for example when a player missed a dart
        /// </summary>
        public void ForcePlayerChange()
        {
            for (; Mode.CurrentPlayerRound.Darts.Count < GameMode.DartsPerTurn;)
            {
                registerDart(0, 0);
            }
        }

        public override void HandleInput(InputState inputState)
        {
            if (inputState.MenuCancel)
            {
                pause();
            }

            if (inputState.IsKeyPressed(Keys.F6))
            {
                _showDartboard = !_showDartboard;
            }

            if (_showDartboard)
            {
                _dartboard.HandleInput(inputState);
            }

            if (inputState.IsKeyPressed(Keys.Space))
            {
                ForcePlayerChange();
            }
        }

        #endregion

        #region StartRound

        public virtual void StartRound()
        {
            // Play new round sound
            playNewRoundSound();
            showNewRoundMessageScreen();
        }

        private void showNewRoundMessageScreen()
        {
            _newRoundTimeoutScreen.Text = "Round " + (Mode.CurrentRoundIndex + 1);
            XnaDartsGame.ScreenManager.AddScreen(_newRoundTimeoutScreen);
        }

        #endregion

        #region StartTurn

        private void startTurn()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.ThrowStart);
            showThrowDartsMessage();
        }

        private void showThrowDartsMessage()
        {
            _throwDartsScreen.Text = Mode.CurrentPlayer.Name + " throw darts!";
            XnaDartsGame.ScreenManager.AddScreen(_throwDartsScreen);
        }

        #endregion

        #region EndTurn

        protected virtual void HandleEndOfTurn()
        {
            if (Mode.IsGameOver())
            {
                HandleGameOver();
            }
            else
            {
                ShowPlayerChangeScreen();
            }
        }

        protected virtual void HandleGameOver()
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.Won);

            var leaders = Mode.GetLeaders();

            var text = "Winner";

            if (leaders.Count > 1)
            {
                text = "Draw";
            }

            leaders.ForEach(p => text += " " + p.Name);

            var gameOverScreen = new MessageBoxScreen("Game Over", text, MessageBoxButtons.Ok);
            gameOverScreen.OnOk += delegate { pause(); };
            XnaDartsGame.ScreenManager.AddScreen(gameOverScreen);
        }

        protected void ShowPlayerChangeScreen()
        {
            _playerChangeScreen.Timeout = TimeSpan.FromSeconds(
                XnaDartsGame.Options.PlayerChangeTimeout
                );
            XnaDartsGame.ScreenManager.AddScreen(_playerChangeScreen);
        }

        #endregion
    }
}