using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes.Cricket;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class CricketMarksComponent : IDrawableGameComponent
    {
        #region Constructor

        public CricketMarksComponent(Cricket mode)
        {
            _mode = mode;
        }

        #endregion

        #region LoadContent

        public void LoadContent(ContentManager content)
        {
            for (var i = 0; i < 4; i++)
            {
                _markTexture[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"Marks\Mark" + i);
            }
            _markTextureSize = new Vector2(_markTexture[0].Width, _markTexture[0].Height);

            for (var i = 0; i < 6; i++)
            {
                _numberTextures[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\" +
                                            (i + 15));
            }

            _numberTextureSize = new Vector2(_numberTextures[0].Width, _numberTextures[0].Height);

            _bullTexture =
                content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\Bull");
            _closedTexture =
                content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\Closed");
            _closedTextureSize = new Vector2(_closedTexture.Width, _closedTexture.Height);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            const int defaultNumberOfSegments = 7;

            var viewport = new Vector2(ResolutionHandler.VWidth, ResolutionHandler.VHeight);
            var componentCenter = viewport*new Vector2(0.5f, 0.4f);

            var markScaling = defaultNumberOfSegments/(float) _mode.Segments.Length*Scale;

            var marksPaddingY = _markTextureSize.Y*0.1f;
            var marksHeight = _markTextureSize.Y + marksPaddingY;
            var marksSpacingY = (marksHeight + marksPaddingY)*markScaling;
            var marksOffsetY = marksSpacingY*_mode.Segments.Length*0.5f;


            var panelWidth = marksSpacingY*1.1f;
            var panelHeight = marksSpacingY*_mode.Segments.Length + marksSpacingY*0.2f;

            var panelPaddingX = panelWidth*0.5f;
            var panelSpacingX = panelWidth + panelPaddingX;


            var numberOfPlayersPanelOffset = -panelSpacingX;

            if (_mode.Players.Count > 2)
            {
                numberOfPlayersPanelOffset *= 2;
            }

            for (var i = 0; i < _mode.Players.Count; i++)
            {
                var player = _mode.Players[i];

                var panelColor = _getPlayerPanelColor(player);

                var currentPanelOffsetX = i*panelSpacingX;
                var panelCenter = componentCenter +
                                  new Vector2(numberOfPlayersPanelOffset + currentPanelOffsetX, 0);

                if (i == 1 && _mode.Players.Count == 2)
                {
                    panelCenter.X += panelSpacingX;
                }
                else if (i > 1 && _mode.Players.Count > 2)
                {
                    panelCenter.X += panelSpacingX;
                }

                var panelRectangle = new Rectangle(
                    (int) (panelCenter.X - panelWidth*0.5f),
                    (int) (panelCenter.Y - panelHeight*0.5f),
                    (int) panelWidth,
                    (int) panelHeight
                    );

                spriteBatch.Draw(ScreenManager.BlankTexture, panelRectangle, panelColor);

                for (var j = 0; j < _mode.Segments.Length; j++)
                {
                    var segment = _mode.Segments[j];
                    var currentMarkOffsetY = marksSpacingY*j;
                    var markCenter = panelCenter +
                                     new Vector2(0, -marksOffsetY + currentMarkOffsetY + marksSpacingY*0.5f);
                    _drawMark(spriteBatch, segment, player, markCenter, markScaling);
                }
            }

            for (var i = 0; i < _mode.Segments.Length; i++)
            {
                var currentNumberOffsetY = marksSpacingY*i;
                var numberCenter = componentCenter +
                                   new Vector2(0, -marksOffsetY + currentNumberOffsetY + marksSpacingY*0.5f);
                var segment = _mode.Segments[i];
                _drawNumber(spriteBatch, segment, numberCenter);
            }
        }

        private Color _getPlayerPanelColor(Player player)
        {
            var panelColor = _mode.GetPlayerColor(player)*0.33f;

            if (player != _mode.CurrentPlayer)
            {
                panelColor *= 0.5f;
            }
            return panelColor;
        }

        private void _drawNumber(SpriteBatch spriteBatch, int segment, Vector2 numberCenter)
        {
            var numberOffset = _numberTextureSize*0.5f;
            Texture2D numberTexture;
            var segmentColor = Color.White;

            if (!_mode.IsSegmentOpen(segment))
            {
                segmentColor *= 0.33f;
            }

            if (segment == 25)
            {
                numberTexture = _bullTexture;
            }
            else
            {
                numberTexture = _numberTextures[segment - 15];
            }

            spriteBatch.Draw(numberTexture, numberCenter - numberOffset, segmentColor);

            if (!_mode.IsSegmentOpen(segment))
            {
                var closedTextureOffset = _closedTextureSize*0.5f;
                spriteBatch.Draw(_closedTexture, numberCenter - closedTextureOffset, Color.White);
            }
        }

        private void _drawMark(SpriteBatch spriteBatch, int segment, Player player, Vector2 center,
            float scaling)
        {
            var marks = _mode.GetScoredMarks(player, segment);
            var segmentColor = Color.White;

            if (marks > 0)
            {
                if (!_mode.IsSegmentOpen(segment))
                {
                    segmentColor *= 0.33f;
                }

                spriteBatch.Draw(_markTexture[Math.Min(marks, 3)], center, null, segmentColor, 0,
                    _markTextureSize*0.5f, scaling, SpriteEffects.None, 0);
            }
        }

        #endregion

        #region Fields and Properties

        private const float Scale = 0.66f;
        private Texture2D _bullTexture;
        private Texture2D _closedTexture;
        private Vector2 _closedTextureSize;
        private Vector2 _markTextureSize;
        private Vector2 _numberTextureSize;
        private readonly Texture2D[] _markTexture = new Texture2D[4];
        private readonly Cricket _mode;
        private readonly Texture2D[] _numberTextures = new Texture2D[6];

        #endregion
    }
}