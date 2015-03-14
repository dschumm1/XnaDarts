using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class CricketMarksComponent : IDrawableGameComponent
    {
        private const float Scale = 0.66f;
        private Texture2D _bullTexture;
        private Texture2D _closedTexture;
        private Vector2 _closedTextureSize;
        private Vector2 _markTextureSize;
        private Vector2 _numberTextureSize;
        private readonly Texture2D[] _markTexture = new Texture2D[4];
        private readonly Cricket _mode;
        private readonly Texture2D[] _numberTextures = new Texture2D[6];

        public CricketMarksComponent(Cricket mode)
        {
            _mode = mode;
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            const int defaultNumberOfSegments = 7;
            var scaling = defaultNumberOfSegments/(float) _mode.Segments.Count*Scale;

            var marksPadding = _markTextureSize.Y*0.1f;
            var marksHeight = _markTextureSize.Y + marksPadding;
            var marksSpacing = marksHeight*scaling + marksPadding;

            var viewport = new Vector2(XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height);
            var marksCenter = viewport*new Vector2(0.5f, 0.4f);
            var marksOffset = marksSpacing*(_mode.Segments.Count - 1)*0.5f;

            var panelPadding = marksSpacing;
            var panelWidth = marksSpacing*1.1f;
            var panelSpacing = panelWidth + panelPadding;
            var panelHeight = marksSpacing*_mode.Segments.Count + marksSpacing*0.2f;
            var panelOffset = panelSpacing*(_mode.Players.Count - 1)*0.5f*Vector2.UnitX;

            for (var i = 0; i < _mode.Players.Count; i++)
            {
                var player = _mode.Players[i];
                var panelColor = _mode.GetPlayerColor(player)*0.33f;
                var panelPosition = marksCenter - panelOffset + panelSpacing*i*Vector2.UnitX;

                var rectangle = new Rectangle(
                    (int) (panelPosition.X - panelWidth*0.5f),
                    (int) (panelPosition.Y - panelHeight*0.5f),
                    (int) panelWidth,
                    (int) panelHeight
                    );

                spriteBatch.Draw(ScreenManager.BlankTexture, rectangle, panelColor);

                for (var j = 0; j < _mode.Segments.Count; j++)
                {
                    var segment = _mode.Segments[j];
                    var markPosition = marksCenter - new Vector2(panelOffset.X, marksOffset) +
                                       new Vector2(panelSpacing * i, marksSpacing * j);
                    drawMark(spriteBatch, segment, player, markPosition, scaling);
                }
            }

            for (var i = 0; i < _mode.Segments.Count; i++)
            {
                var numberPosition = marksCenter - new Vector2(0, marksOffset) + new Vector2(0, marksSpacing)*i;
                var segment = _mode.Segments[i];
                drawNumber(spriteBatch, segment, numberPosition);
            }
        }

        private void drawNumber(SpriteBatch spriteBatch, CricketSegment segment, Vector2 numberPosition)
        {
            var numberOffset = _numberTextureSize*0.5f;
            Texture2D numberTexture;
            var segmentColor = Color.White;

            if (!segment.IsOpen)
            {
                segmentColor *= 0.33f;
            }

            if (segment.Segment == 25)
            {
                numberTexture = _bullTexture;
            }
            else
            {
                numberTexture = _numberTextures[segment.Segment - 15];
            }

            spriteBatch.Draw(numberTexture, numberPosition - numberOffset, segmentColor);

            if (!segment.IsOpen)
            {
                var closedTextureOffset = _closedTextureSize*0.5f;
                spriteBatch.Draw(_closedTexture, numberPosition - closedTextureOffset, Color.White);
            }
        }

        private void drawMark(SpriteBatch spriteBatch, CricketSegment segment, Player player, Vector2 position,
            float scaling)
        {
            var marks = segment.GetScoredMarks(player);
            var segmentColor = Color.White;

            if (marks > 0)
            {
                if (!segment.IsOpen)
                {
                    segmentColor *= 0.33f;
                }

                spriteBatch.Draw(_markTexture[Math.Min(marks, 3)], position, null, segmentColor, 0,
                    _markTextureSize*0.5f, scaling, SpriteEffects.None, 0);
            }
        }
    }
}