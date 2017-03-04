using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Screens.Menus;

namespace XnaDarts.ScreenManagement
{
    public enum StackPanelOrientation
    {
        Horizontal,
        Vertical
    }

    public abstract class MenuScreen : GameScreen
    {
        private ContentManager _content;
        private bool _lastIsCoveredByOtherScreen;
        //private Curve _myCurve;
        private int _selectedEntry;
        private float _transitionPosition = 2;
        private float _transitionTimer;
        public StackPanel MenuItems = new StackPanel();
        public Vector2 MenuPosition;
        public int PaddingX = 24;
        public int PaddingY = 8;
        public StackPanel StackPanel = new StackPanel();

        internal MenuScreen(string title)
        {
            TransitionDuration = 0.5f;
            MenuPosition = new Vector2(100, 100);
            StackPanel.Items.Add(new TextBlock(title) {Color = Color.LightBlue, Font = ScreenManager.Trebuchet32});
            StackPanel.Items.Add(MenuItems);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, "Content");

            //_myCurve = _content.Load<Curve>(@"Curves\MenuCurve");

            if (MenuItems.Items.Count > 0)
            {
                _selectedEntry = 0;
                ((MenuEntry) MenuItems.Items[_selectedEntry]).Color = XnaDartsColors.SelectedMenuItemForeground;
            }
        }

        public override void HandleInput(InputState inputState)
        {
            var oldSelectedEntry = _selectedEntry;

            _handleKeyboardInput(inputState);
            _handleMouseInput(inputState);

            if (oldSelectedEntry != _selectedEntry)
            {
                ((MenuEntry) MenuItems.Items[oldSelectedEntry]).Color = XnaDartsColors.MenuItemForeground;
                ((MenuEntry) MenuItems.Items[_selectedEntry]).Color = XnaDartsColors.SelectedMenuItemForeground;
                XnaDartsGame.SoundManager.PlaySound(SoundCue.MenuSelect);
            }
        }

        /// <summary>
        ///     Temporary(or not? :D) solution for handling mouse input
        /// </summary>
        /// <param name="inputState"></param>
        private void _handleMouseInput(InputState inputState)
        {
            var height = 0;

            var mousePosition = new Vector2(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y);
            var viewportOffset = new Vector2(ResolutionHandler.ViewportX, ResolutionHandler.ViewportY);
            var mouseInGameCoords = Vector2.Transform(mousePosition - viewportOffset,
                Matrix.Invert(ResolutionHandler.GetTransformationMatrix()));

            for (var i = 0; i < StackPanel.Items.Count; i++)
            {
                if (StackPanel.Items[i] == MenuItems)
                {
                    for (var j = 0; j < MenuItems.Items.Count; j++)
                    {
                        var menuItemBoundingBox =
                            new Rectangle(
                                (int) MenuPosition.X,
                                (int) (MenuPosition.Y + height),
                                MenuItems.Items[j].Width,
                                MenuItems.Items[j].Height);
                        if (menuItemBoundingBox.Contains(mouseInGameCoords.X, mouseInGameCoords.Y))
                        {
                            _selectedEntry = j;

                            if (inputState.MouseClick)
                                ((MenuEntry) MenuItems.Items[j]).Select();
                            else if (inputState.MouseRightClick)
                                ((MenuEntry) MenuItems.Items[j]).Cancel();

                            break;
                        }
                        height += MenuItems.Items[j].Height;
                    }
                    break;
                }
                height += StackPanel.Items[i].Height;
            }
        }

        private void _handleKeyboardInput(InputState inputState)
        {
            var selectedMenuEntry = MenuItems.Items[_selectedEntry];
            selectedMenuEntry.HandleInput(inputState);

            if (inputState.MenuDown)
                _selectedEntry++;
            if (inputState.MenuUp)
                _selectedEntry--;

            if (_selectedEntry > MenuItems.Items.Count - 1)
                _selectedEntry = 0;
            if (_selectedEntry < 0)
                _selectedEntry = MenuItems.Items.Count - 1;

            if (inputState.MenuCancel)
            {
                XnaDartsGame.SoundManager.PlaySound(SoundCue.MenuBack);
                CancelScreen();
            }
        }

        public virtual void CancelScreen()
        {
            State = ScreenState.Exiting;
            _transitionTimer = 0;
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            var dt = GetDeltaTimeInSeconds(gameTime);

            _transitionTimer += dt;

            // Screen went from being on top to being covered
            if (_lastIsCoveredByOtherScreen != isCoveredByOtherScreen)
                _transitionTimer = 0;
            _lastIsCoveredByOtherScreen = isCoveredByOtherScreen;

            base.Update(gameTime, isCoveredByOtherScreen);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var transitionTo = 0;

            if (IsCoveredByOtherScreen)
                transitionTo = 0;
            else if (State == ScreenState.Active || State == ScreenState.Entering)
                transitionTo = 1;
            else if (State == ScreenState.Exiting)
                transitionTo = 2;

            _transitionPosition += (transitionTo - _transitionPosition) *
                                   MathHelper.Lerp(0, 1, MathHelper.Clamp(_transitionTimer, 0, 1));

            var stackPanelPosition = new Vector2
            {
                X = _transitionPosition * MenuPosition.X,
                Y = MenuPosition.Y
            };

            var sin = ((float) Math.Sin(ElapsedTime * 5f) + 1.0f) / 2.0f;
            var alpha = sin * 0.8f + 0.2f;
            var c = Color.Lerp(Color.White, XnaDartsColors.SelectedMenuItemForeground, alpha);
            ((MenuEntry) MenuItems.Items[_selectedEntry]).Color = c;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                ResolutionHandler.GetTransformationMatrix());
            StackPanel.Draw(spriteBatch, stackPanelPosition, TransitionAlpha);
            spriteBatch.End();
        }
    }
}