using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class SegmentMapScreen : MenuScreen
    {
        private readonly MenuEntry _back = new MenuEntry("Back");
        private readonly MenuEntry _bindAll = new MenuEntry("Bind All");
        private readonly MenuEntry _clear = new MenuEntry("Clear All Bindings");
        private readonly MenuEntry _edit = new MenuEntry("Edit Bindings");
        private readonly int[] _segmentOrder = {20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5};
        private readonly int[] _segmentRotation = {1, 8, 10, 3, 19, 5, 12, 14, 17, 6, 15, 18, 4, 16, 7, 13, 9, 2, 11, 0};
        private BindSegmentScreen _bindScreen;
        private Vector2 _boardPosition;
        private float _boardScale = 0.33f;
        private float _distance;
        private Texture2D _doubleBullTexture;
        private Texture2D _doubleTexture;
        private bool _drawCoords;
        private bool _hasMadeChanges;
        private bool _isEditing;
        private Texture2D _mapTexture;
        private IntPair _mouseOver;
        private Vector2 _mouseVector = Vector2.Zero;
        private Dictionary<IntPair, IntPair> _segmentMap;
        private Texture2D _segmentTexture;

        /// <summary>
        ///     0 = Single, 1 = Double, 2 = Triple, 3 = Single Bull, 4 = Double Bull
        /// </summary>
        private Texture2D[] _segmentTextures;

        private IntPair _selectedSegment = new IntPair(0, 1);
        private Texture2D _singleBullTexture;
        private Texture2D _tripleTexture;
        public ContentManager Content;

        public SegmentMapScreen(string title) : base(title)
        {
            _edit.OnSelected += edit_OnSelected;
            _clear.OnSelected += clear_OnSelected;
            _back.OnSelected += back_OnSelected;
            _bindAll.OnSelected += bindAll_OnSelected;

            MenuItems.AddItems(_bindAll, _edit, _clear, _back);

            if (XnaDartsGame.Options.SegmentMap.Any())
            {
                _segmentMap = new Dictionary<IntPair, IntPair>(XnaDartsGame.Options.SegmentMap);
            }
            else
            {
                buildSegmentMap();
            }
        }

        private void bindAll_OnSelected(object sender, EventArgs e)
        {
            _selectedSegment = new IntPair(0, 1);
            showBindSegmentScreen(getSelectedSegment(), true);
        }

        private void edit_OnSelected(object sender, EventArgs e)
        {
            _isEditing = true;
        }

        private void clear_OnSelected(object sender, EventArgs e)
        {
            var mb = new MessageBoxScreen("Confirm", "Are you sure you want to clear all bindings?",
                MessageBoxButtons.Yes | MessageBoxButtons.No);
            mb.OnYes += mbClear_OnYes;
            XnaDartsGame.ScreenManager.AddScreen(mb);
        }

        private void mbClear_OnYes(object sender, EventArgs e)
        {
            buildSegmentMap();
            _hasMadeChanges = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (Content == null)
            {
                Content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, "Content");
            }

            _mapTexture = Content.Load<Texture2D>(@"Images\SegmentMap");

            _boardScale = 0.8f*XnaDartsGame.Viewport.Height/_mapTexture.Height;

            _boardPosition = new Vector2(XnaDartsGame.Viewport.Width - _boardScale*_mapTexture.Width*0.5f - 20.0f,
                XnaDartsGame.Viewport.Height*0.5f);

            _segmentTexture = Content.Load<Texture2D>(@"Images\Segment");
            _tripleTexture = Content.Load<Texture2D>(@"Images\Triple");
            _doubleTexture = Content.Load<Texture2D>(@"Images\Double");
            _singleBullTexture = Content.Load<Texture2D>(@"Images\SingleBull");
            _doubleBullTexture = Content.Load<Texture2D>(@"Images\DoubleBull");

            _segmentTextures = new[]
            {_segmentTexture, _doubleTexture, _tripleTexture, _singleBullTexture, _doubleBullTexture};
        }

        private void back_OnSelected(object sender, EventArgs e)
        {
            if (_hasMadeChanges)
            {
                var mb = new MessageBoxScreen("Confirm", "Do you want to save the changes?",
                    MessageBoxButtons.Yes | MessageBoxButtons.No | MessageBoxButtons.Cancel);
                mb.OnYes += mb_OnYes;
                mb.OnNo += mb_OnNo;
                XnaDartsGame.ScreenManager.AddScreen(mb);
            }
            else
            {
                base.CancelScreen();
            }
        }

        private void mb_OnNo(object sender, EventArgs e)
        {
            base.CancelScreen();
        }

        private void mb_OnYes(object sender, EventArgs e)
        {
            XnaDartsGame.Options.SegmentMap = _segmentMap;
            base.CancelScreen();
        }

        public override void CancelScreen()
        {
            back_OnSelected(null, null);
        }

        private void buildSegmentMap()
        {
            _segmentMap = new Dictionary<IntPair, IntPair>();

            for (var i = 1; i <= 20; i++)
            {
                for (var j = 1; j <= 3; j++)
                {
                    _segmentMap.Add(new IntPair(i, j), null);
                }
            }

            _segmentMap.Add(new IntPair(25, 1), null);
            _segmentMap.Add(new IntPair(25, 2), null);
        }

        public override void HandleInput(InputState inputState)
        {
            if (!_isEditing)
            {
                base.HandleInput(inputState);
            }
            else
            {
                if (inputState.MenuCancel)
                {
                    _isEditing = false;
                }
                if (inputState.MenuRight)
                {
                    _selectedSegment.X++;
                }
                if (inputState.MenuLeft)
                {
                    _selectedSegment.X--;
                }
                if (inputState.MenuUp)
                {
                    _selectedSegment.Y++;
                }
                if (inputState.MenuDown)
                {
                    _selectedSegment.Y--;
                }

                if (inputState.MenuEnter)
                {
                    showBindSegmentScreen(getSelectedSegment(), false);
                }

                if (_selectedSegment.X > 19)
                {
                    _selectedSegment.X = 0;
                }
                if (_selectedSegment.Y > 5)
                {
                    _selectedSegment.Y = 1;
                }
                if (_selectedSegment.X < 0)
                {
                    _selectedSegment.X = 19;
                }
                if (_selectedSegment.Y < 1)
                {
                    _selectedSegment.Y = 5;
                }
            }

            var boardRectangle = new Rectangle((int) (_boardPosition.X - _boardScale*_mapTexture.Width*0.5 - 20.0f),
                (int) (_boardPosition.Y - _boardScale*_mapTexture.Height*0.5),
                (int) (_mapTexture.Width*_boardScale),
                (int) (_mapTexture.Height*_boardScale));

            _drawCoords = false;
            _mouseOver = null;
            _mouseVector = new Vector2(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y);

            if (boardRectangle.Contains(inputState.CurrentMouseState.X, inputState.CurrentMouseState.Y))
            {
                var dx = inputState.CurrentMouseState.X -
                         (int) (_boardPosition.X - _boardScale*_mapTexture.Width*0.5f - 20.0f);
                var dy = inputState.CurrentMouseState.Y - (int) (_boardPosition.Y - _boardScale*_mapTexture.Height*0.5f);

                var rotation = (float) Math.Atan2(dy, dx);

                var tempVector = _boardPosition - _mouseVector;
                _distance = tempVector.Length();
                tempVector.Normalize();

                var segmentVector = Vector2.UnitY;
                var rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(18));

                if (_distance < 350.0f*_boardScale) //Inside dartboard
                {
                    _drawCoords = true;

                    if (_distance < 15.0f*_boardScale) //Double bull
                    {
                        _mouseOver = new IntPair(25, 2);
                    }
                    else if (_distance < 40.0f*_boardScale) //Single bull
                    {
                        _mouseOver = new IntPair(25, 1);
                    }
                    else
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            var angle = (float) Math.Acos(Vector2.Dot(segmentVector, tempVector));

                            var tripleRadius = 190.0f*_boardScale;
                            var doubleRadius = 320.0f*_boardScale;

                            if (Math.Abs(angle) < MathHelper.ToRadians(9.0f))
                            {
                                _mouseOver = new IntPair(_segmentOrder[i], 0);

                                if (_distance > tripleRadius && _distance < tripleRadius + 30.0f*_boardScale)
                                    // Triple
                                {
                                    _mouseOver.Y = 3;
                                }
                                else if (_distance > doubleRadius) //Double
                                {
                                    _mouseOver.Y = 2;
                                }
                                else
                                {
                                    _mouseOver.Y = 1;
                                }

                                break;
                            }

                            segmentVector = Vector2.Transform(segmentVector, rotationMatrix);
                        }
                    }

                    if (inputState.MouseClick)
                    {
                        if (_mouseOver != null)
                        {
                            showBindSegmentScreen(_mouseOver, false);
                        }
                    }
                }
            }
        }

        private IntPair getSelectedSegment()
        {
            var segment = new IntPair(_segmentOrder[_selectedSegment.X], _selectedSegment.Y);

            // If the selected multiplier is 4 or 5, select the bullseye
            if (segment.Y == 4 || segment.Y == 5)
            {
                segment.X = 25;
                segment.Y = segment.Y - 3;
            }

            return segment;
        }

        private void showBindSegmentScreen(IntPair segment, bool bindingAll)
        {
            var prefix = "Single";

            if (segment.Y == 2)
            {
                prefix = "Double";
            }
            else if (segment.Y == 3)
            {
                prefix = "Triple";
            }

            var text = "Press " + prefix + " " + segment.X;

            _bindScreen = new BindSegmentScreen(text, segment);
            _bindScreen.OnDartHit += bindScreen_OnDartHit;
            _bindScreen.OnClear += bindScreen_OnClear;

            if (bindingAll)
            {
                _bindScreen.OnDartHit += bindScreen_BindNext;
                _bindScreen.OnClear += bindScreen_BindNext;
                _bindScreen.OnCancel += bindScreen_BindNext;
            }

            XnaDartsGame.ScreenManager.AddScreen(_bindScreen);
        }

        private void bindScreen_BindNext(object sender, EventArgs e)
        {
            var temp = true;

            if (_selectedSegment.Y < 4)
            {
                _selectedSegment.X++;
                if (_selectedSegment.X > 19)
                {
                    _selectedSegment.X = 0;
                    _selectedSegment.Y++;
                }
            }
            else
            {
                _selectedSegment.Y++;
                temp = false;
            }

            showBindSegmentScreen(getSelectedSegment(), temp);
        }

        private void bindScreen_OnClear(object sender, EventArgs e)
        {
            _segmentMap[_bindScreen.SelectedSegment] = null;
            _hasMadeChanges = true;
        }

        private void bindScreen_OnDartHit(object sender, EventArgs e)
        {
            XnaDartsGame.SoundManager.PlaySound(SoundCue.Single);
            _segmentMap[_bindScreen.SelectedSegment] = _bindScreen.SegmentCoordinates;
            _hasMadeChanges = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            drawDartboard(spriteBatch);

            drawBoardSegments(spriteBatch);

            highlightSegmentUnderCursor(spriteBatch);

            highlightSelectedSegment(spriteBatch);

            spriteBatch.End();
        }

        private void drawDartboard(SpriteBatch spriteBatch)
        {
            var offset = new Vector2(_mapTexture.Width, _mapTexture.Height)*0.5f;
            var position = _boardPosition;
            spriteBatch.Draw(_mapTexture, position, null, Color.White, 0, offset, _boardScale, SpriteEffects.None, 0);
        }

        private void highlightSelectedSegment(SpriteBatch spriteBatch)
        {
            if (_isEditing)
            {
                var position = _boardPosition;
                var offset = new Vector2(_mapTexture.Width, _mapTexture.Height)*0.5f;
                float rotation = _segmentRotation[_segmentOrder[_selectedSegment.X] - 1];
                var index = _selectedSegment.Y - 1;

                if (_selectedSegment.Y == 4)
                {
                    index = 3;
                }
                else if (_selectedSegment.Y == 5)
                {
                    index = 4;
                }

                spriteBatch.Draw(_segmentTextures[index], position, null, Color.White*0.33f,
                    MathHelper.ToRadians(rotation*18.0f), offset, _boardScale, SpriteEffects.None, 0);
            }
        }

        private void highlightSegmentUnderCursor(SpriteBatch spriteBatch)
        {
            if (_mouseOver != null)
            {
                string text;
                if (_segmentMap[_mouseOver] != null)
                {
                    text = _segmentMap[_mouseOver].ToString();
                }
                else
                {
                    text = "Not Set";
                }

                var position = _mouseVector + new Vector2(80, 40);
                var offset = ScreenManager.Trebuchet24.MeasureString(text)*0.5f;

                if (_drawCoords)
                {
                    spriteBatch.DrawString(ScreenManager.Trebuchet24, text, position - offset, Color.White);

                    position = _boardPosition;

                    float rotation;
                    if (_mouseOver.X != 25)
                    {
                        rotation = _segmentRotation[_mouseOver.X - 1];
                    }
                    else
                    {
                        rotation = 0;
                    }

                    offset = new Vector2(_mapTexture.Width, _mapTexture.Height)*0.5f;
                    var textureIndex = _mouseOver.Y - 1;

                    if (_mouseOver.X == 25 && _mouseOver.Y == 1)
                    {
                        textureIndex = 3;
                    }
                    else if (_mouseOver.X == 25 && _mouseOver.Y == 2)
                    {
                        textureIndex = 4;
                    }

                    spriteBatch.Draw(_segmentTextures[textureIndex], position, null, Color.White*0.33f,
                        MathHelper.ToRadians(rotation*18.0f), offset, _boardScale, SpriteEffects.None, 0);
                }
            }
        }

        private void drawBoardSegments(SpriteBatch spriteBatch)
        {
            var position = _boardPosition;

            var assignedSegmentColor = Color.Lime*0.33f;
            var unassignedSegmentColor = Color.White*0; //0.33f;

            foreach (var p in _segmentMap)
            {
                Color segmentColor;

                if (p.Value == null)
                {
                    segmentColor = unassignedSegmentColor;
                }
                else
                {
                    segmentColor = assignedSegmentColor;
                }

                float rotation = 0;

                int textureIndex;

                if (p.Key.X == 25)
                {
                    textureIndex = 2 + p.Key.Y;
                }
                else
                {
                    textureIndex = p.Key.Y - 1;
                    rotation = _segmentRotation[p.Key.X - 1];
                }

                var offset = new Vector2(_mapTexture.Width, _mapTexture.Height)*0.5f;
                spriteBatch.Draw(_segmentTextures[textureIndex], position, null, segmentColor,
                    MathHelper.ToRadians(rotation*18.0f), offset,
                    _boardScale, SpriteEffects.None, 0);
            }
        }
    }
}