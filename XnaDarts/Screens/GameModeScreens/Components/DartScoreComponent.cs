using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class DartScoreComponent : IDrawableGameComponent
    {
        private const float DartBlinkRate = 0.5f;
        private Vector2 _dartTextureSize;
        private Vector2 _numberTextureSize;
        private Texture2D _solidDart;
        public Texture2D DartTexture;
        private readonly GameMode _mode;
        private readonly Texture2D[] _numberTextures = new Texture2D[3];
        public bool Vertical;
        public Vector2 Position;

        public DartScoreComponent(GameMode mode)
        {
            _mode = mode;
            Position = new Vector2(0.5f, 0.7f);
        }

        public void LoadContent(ContentManager content)
        {
            DartTexture = content.Load<Texture2D>(@"Images\DartStroke");
            _solidDart = content.Load<Texture2D>(@"Images\DartHighlight");
            _dartTextureSize = new Vector2(DartTexture.Width, DartTexture.Height);

            for (var i = 0; i < 3; i++)
            {
                _numberTextures[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\" +
                                            (i + 1));
            }

            _numberTextureSize = new Vector2(_numberTextures[0].Width, _numberTextures[0].Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var viewport = new Vector2(ResolutionHandler.VWidth, ResolutionHandler.VHeight);
            var centerOfDarts = viewport*Position;

            Vector2 orientation;

            if (Vertical)
            {
                orientation = Vector2.UnitY;
            }
            else
            {
                orientation = Vector2.UnitX;
            }

            var dartsPadding = 0.6f*_dartTextureSize;
            var dartsSpacing = _dartTextureSize + dartsPadding;
            var dartsOffset = dartsSpacing*(GameMode.DartsPerTurn - 1)*0.5f * orientation;

            //Draw images
            for (var i = 0; i < GameMode.DartsPerTurn; i++)
            {
                var dartPosition = centerOfDarts - dartsOffset + dartsSpacing * i * orientation;

                if (i < _mode.CurrentPlayerRound.Darts.Count)
                {
                    var dart = _mode.CurrentPlayerRound.Darts[i];
                    _drawDartScoreInText(spriteBatch, dart, dartPosition);
                }
                else
                {
                    _drawSolidDart(spriteBatch, dartPosition);

                    var isCurrentDartToThrow = i == _mode.CurrentPlayerRound.Darts.Count;
                    if (isCurrentDartToThrow)
                    {
                        _drawBlinkingDart(spriteBatch, dartPosition);
                    }

                    _drawNumber(spriteBatch, i, dartPosition);
                }
            }
        }

        private void _drawDartScoreInText(SpriteBatch spriteBatch, Dart dart, Vector2 dartPosition)
        {
            string text;
            Color color;
            dart.GetVerbose(out text, out color);

            var textOffset = ScreenManager.Trebuchet32.MeasureString(text)*0.5f;
            TextBlock.DrawShadowed(spriteBatch, ScreenManager.Trebuchet32, text, color,
                dartPosition - textOffset);
        }

        private void _drawNumber(SpriteBatch spriteBatch, int i, Vector2 position)
        {
            var numberOffset = _numberTextureSize*0.5f;
            spriteBatch.Draw(_numberTextures[i], position - numberOffset, Color.White);
        }

        private void _drawSolidDart(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(DartTexture, position - _dartTextureSize*0.5f, Color.White);
        }

        private void _drawBlinkingDart(SpriteBatch spriteBatch, Vector2 position)
        {
            var alpha = ((float) (Math.Sin(XnaDartsGame.ElapsedTime*MathHelper.Pi/DartBlinkRate)) + 1)*0.5f;
            spriteBatch.Draw(_solidDart, position - _dartTextureSize*0.5f, Color.White*alpha);
        }
    }
}