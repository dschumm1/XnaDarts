using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class CricketRoundMarksComponent : IDrawableGameComponent
    {
        private const float Scale = 0.25f;
        private Vector2 _offset;
        private float _spacing;
        private Vector2 _tempPosition;
        private readonly SpriteFont _font = ScreenManager.Trebuchet24;
        private readonly Texture2D[] _markTextures = new Texture2D[4];
        private readonly Cricket _mode;
        private readonly Vector2 _position;

        public CricketRoundMarksComponent(Cricket mode)
        {
            _mode = mode;
            _position = new Vector2(20, XnaDartsGame.Viewport.Height*0.4f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tempPosition = _position;
            foreach (var round in _mode.CurrentPlayer.Rounds)
            {
                drawRoundMarks(spriteBatch, round);
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

        private void drawRoundMarks(SpriteBatch spriteBatch, Round round)
        {
            foreach (var dart in round.Darts)
            {
                drawDartMarks(spriteBatch, dart);
                _tempPosition.X += _spacing;
            }
        }

        private void drawDartMarks(SpriteBatch spriteBatch, Dart dart)
        {
            var scoredMarks = ((CricketDart) dart).ScoredMarks;
            var markTexture = _markTextures[Math.Min(scoredMarks, 3)];

            spriteBatch.Draw(markTexture, _tempPosition, null, Color.White, 0, _offset, Scale, SpriteEffects.None, 0);
        }
    }
}