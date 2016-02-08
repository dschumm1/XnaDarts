using Microsoft.Xna.Framework.Input;

namespace XnaDarts.ScreenManagement
{
    public class InputState
    {
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboarstState;
        private MouseState _lastMouseState;
        public bool[] CurrentBoardButtonStates;
        public MouseState CurrentMouseState;
        public bool[] LastBoardButtonStates;

        public bool MouseMove
        {
            get { return CurrentMouseState.X != _lastMouseState.X || CurrentMouseState.Y != _lastMouseState.Y; }
        }

        public bool MouseClick
        {
            get
            {
                return CurrentMouseState.LeftButton == ButtonState.Pressed &&
                       _lastMouseState.LeftButton == ButtonState.Released;
            }
        }

        public bool MouseRightClick
        {
            get
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed &&
                       _lastMouseState.RightButton == ButtonState.Released;
            }
        }

        public bool MenuDown
        {
            get
            {
                return _currentKeyboardState.IsKeyDown(Keys.Down) && _lastKeyboarstState.IsKeyUp(Keys.Down) ||
                       CurrentBoardButtonStates[0] && LastBoardButtonStates[0] == false;
            }
        }

        public bool MenuBack
        {
            get { return _currentKeyboardState.IsKeyDown(Keys.Back) && _lastKeyboarstState.IsKeyUp(Keys.Back); }
        }

        public bool MenuCancel
        {
            get { return _currentKeyboardState.IsKeyDown(Keys.Escape) && _lastKeyboarstState.IsKeyUp(Keys.Escape); }
        }

        public bool MenuEnter
        {
            get
            {
                return (_currentKeyboardState.IsKeyDown(Keys.Enter) && _lastKeyboarstState.IsKeyUp(Keys.Enter)) ||
                       (_currentKeyboardState.IsKeyDown(Keys.Space) && _lastKeyboarstState.IsKeyUp(Keys.Space)) ||
                       CurrentBoardButtonStates[2] && LastBoardButtonStates[2] == false;
            }
        }

        public bool MenuUp
        {
            get
            {
                return _currentKeyboardState.IsKeyDown(Keys.Up) && _lastKeyboarstState.IsKeyUp(Keys.Up) ||
                       CurrentBoardButtonStates[1] && LastBoardButtonStates[1] == false;
            }
        }

        public bool MenuRight
        {
            get
            {
                return _currentKeyboardState.IsKeyDown(Keys.Right) && _lastKeyboarstState.IsKeyUp(Keys.Right) ||
                       CurrentBoardButtonStates[4] && LastBoardButtonStates[4] == false;
            }
        }

        public bool MenuLeft
        {
            get
            {
                return _currentKeyboardState.IsKeyDown(Keys.Left) && _lastKeyboarstState.IsKeyUp(Keys.Left) ||
                       CurrentBoardButtonStates[3] && LastBoardButtonStates[3] == false;
            }
        }

        public void Update()
        {
            _lastKeyboarstState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            LastBoardButtonStates = CurrentBoardButtonStates;
            CurrentBoardButtonStates = SerialManager.Instance().ButtonStates;
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _lastKeyboarstState.IsKeyUp(key);
        }
    }
}