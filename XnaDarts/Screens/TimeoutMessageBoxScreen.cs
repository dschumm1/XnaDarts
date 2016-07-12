using Microsoft.Xna.Framework;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens
{
    public class TimeoutMessageBoxScreen : MessageBoxScreen
    {
        private readonly float _timeOut;

        public TimeoutMessageBoxScreen(string title, string message, MessageBoxButtons buttons, float timeOut)
            : base(title, message, buttons)
        {
            _timeOut = timeOut;
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            if (ElapsedTime >= _timeOut && State != ScreenState.Exiting)
            {
                meOk_OnSelected(this, null);
            }
        }
    }
}