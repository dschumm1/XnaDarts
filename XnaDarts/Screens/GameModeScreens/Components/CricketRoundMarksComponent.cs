using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes.Cricket;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class CricketRoundMarksComponent : IDrawableGameComponent
    {
        private const float Scale = 0.25f;
        private readonly SpriteFont _font = ScreenManager.Trebuchet24;
        private readonly Texture2D[] _markTextures = new Texture2D[4];
        private readonly Cricket _mode;
        private readonly Vector2 _position;
        private Vector2 _offset;
        private float _spacing;
        private Vector2 _tempPosition;

        public CricketRoundMarksComponent(Cricket mode)
        {
            _mode = mode;
            _position = new Vector2(20, ResolutionHandler.VHeight*0.5f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tempPosition = _position;

            var maxRows = 5;
            var startIndex = Math.Max(0, 1 + _mode.CurrentRoundIndex - maxRows);
            var endIndex = Math.Min(_mode.MaxRounds, startIndex + maxRows);

            for (var i = startIndex; i < endIndex; i++)
            {
                var color = Color.White;
                if (i == _mode.CurrentRoundIndex)
                {
                    color = Color.Yellow;
                }
                else if (i > _mode.CurrentRoundIndex)
                {
                    color *= 0.33f;
                }

                var round = _mode.CurrentPlayer.Rounds[i];
                var text = "R" + (i + 1) + ". ";
                var font = ScreenManager.Trebuchet24;
                var textSize = font.MeasureString(text);
                TextBlock.DrawShadowed(spriteBatch, font, text, color,
                    _tempPosition + new Vector2(0, -textSize.Y*0.225f));
                _tempPosition.X += font.MeasureString(text).X;

                _drawRoundMarks(spriteBatch, round);
                _tempPosition.X = _position.X;
                _tempPosition.Y += _font.LineSpacing;
            }
        }

        public void LoadContent(ContentManager content)
        {
            for (var i = 0; i < 4; i++)
            {
                _markTextures[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"Marks\Mark" + i);
            }
            _offset = new Vector2(_markTextures[0].Width, _markTextures[0].Height)*0.5f*Scale;
            _spacing = _markTextures[0].Width*Scale + 10.0f;
        }

        private void _drawRoundMarks(SpriteBatch spriteBatch, Round round)
        {
            foreach (var dart in round.Darts)
            {
                _drawDartMarks(spriteBatch, dart);
                _tempPosition.X += _spacing;
            }
        }

        private void _drawDartMarks(SpriteBatch spriteBatch, Dart dart)
        {
            var scoredMarks = _mode.GetScoredMarks(dart);
            var markTexture = _markTextures[Math.Min(scoredMarks, 3)];

            spriteBatch.Draw(markTexture, _tempPosition, null, Color.White, 0, _offset, Scale, SpriteEffects.None, 0);
        }
    }
}