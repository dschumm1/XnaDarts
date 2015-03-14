using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts.ScreenManagement
{
    public enum ScreenState
    {
        Entering,
        Active,
        Exiting
    }

    public abstract class GameScreen
    {
        public float ElapsedTime;
        public bool IsCoveredByOtherScreen;
        public ScreenState State = ScreenState.Entering;
        protected float TransitionAlpha;
        protected float TransitionDuration = 0;

        public virtual void LoadContent()
        {
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            IsCoveredByOtherScreen = isCoveredByOtherScreen;

            var dt = GetDeltaTimeInSeconds(gameTime);

            ElapsedTime += dt;

            updateTransitionAlpha(dt, isCoveredByOtherScreen);

            updateScreenState();
        }

        public static float GetDeltaTimeInSeconds(GameTime gameTime)
        {
            var dt = (float) gameTime.ElapsedGameTime.TotalMilliseconds/1000.0f;
            return dt;
        }

        public void RemoveScreen()
        {
            XnaDartsGame.ScreenManager.RemoveScreen(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void HandleInput(InputState inputState)
        {
        }

        private void updateScreenState()
        {
            if (State == ScreenState.Exiting && TransitionAlpha <= 0)
            {
                RemoveScreen();
            }
            else if (State == ScreenState.Entering && TransitionAlpha >= 1.0f)
            {
                State = ScreenState.Active;
            }
        }

        private void updateTransitionAlpha(float deltaTime, bool isCoveredByOtherScreen)
        {
            var transitionDelta = deltaTime/TransitionDuration;

            if (isCoveredByOtherScreen || State == ScreenState.Exiting)
            {
                TransitionAlpha -= transitionDelta;
            }
            else
            {
                TransitionAlpha += transitionDelta;
            }

            TransitionAlpha = MathHelper.Clamp(TransitionAlpha, 0, 1.0f);
        }
    }
}