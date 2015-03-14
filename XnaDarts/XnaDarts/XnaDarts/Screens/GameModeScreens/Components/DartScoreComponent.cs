using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;
using XnaDarts.Screens.Menus;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class DartScoreComponent : IDrawableGameComponent
    {
        private const float DartBlinkRate = 500.0f;
        private Texture2D _solidDart;
        public Texture2D DartTexture;
        private readonly GameMode _mode;
        private readonly Texture2D[] _numberTextures = new Texture2D[3];

        public DartScoreComponent(GameMode mode)
        {
            _mode = mode;
        }

        public void LoadContent(ContentManager content)
        {
            DartTexture = content.Load<Texture2D>(@"Images\DartStroke");
            _solidDart = content.Load<Texture2D>(@"Images\DartHighlight");


            for (var i = 0; i < 3; i++)
            {
                _numberTextures[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\" +
                                            (i + 1));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var dartSize = new Vector2(DartTexture.Width, DartTexture.Height)*1.4f;
            var position = new Vector2(XnaDartsGame.Viewport.Width*0.5f - GameMode.DartsPerTurn*0.5f*dartSize.X,
                XnaDartsGame.Viewport.Height*0.68f);
            DrawDartScore(spriteBatch, dartSize, position, false);
        }

        public virtual void DrawDartScore(SpriteBatch spriteBatch, Vector2 dartSize, Vector2 position, bool vertical)
        {
            Vector2 orientation;

            if (vertical)
            {
                orientation = Vector2.UnitY;
            }
            else
            {
                orientation = Vector2.UnitX;
            }

            var temp = position + dartSize*orientation*_mode.CurrentPlayerRound.Darts.Count;

            //Draw images
            for (var i = _mode.CurrentPlayerRound.Darts.Count; i < GameMode.DartsPerTurn; i++)
            {
                //Blinking dart
                var numberOffset = drawBlinkingDart(spriteBatch, dartSize, temp, i);

                if (XnaDartsGame.Options.Debug)
                {
                    numberOffset = ScreenManager.Trebuchet24.MeasureString((i + 1).ToString())*0.5f;
                    spriteBatch.DrawString(ScreenManager.Trebuchet24, (i + 1).ToString(),
                        temp + dartSize*0.5f - numberOffset, Color.White);
                }

                spriteBatch.Draw(_numberTextures[i], temp + dartSize*0.5f - numberOffset, Color.White);

                temp += dartSize*orientation;
            }

            temp = position;
            var center = dartSize*0.5f;

            // Draw dart score
            foreach (var dart in _mode.CurrentPlayerRound.Darts)
            {
                string text;
                Color c;
                dart.GetVerbose(out text, out c);

                var offset = ScreenManager.Trebuchet32.MeasureString(text)*0.5f;
                TextBlock.DrawShadowed(spriteBatch, ScreenManager.Trebuchet32, text, c, temp + center - offset);
                temp += dartSize*orientation;
            }
        }

        private Vector2 drawBlinkingDart(SpriteBatch spriteBatch, Vector2 dartSize, Vector2 temp, int i)
        {
            var dartTextureSize = new Vector2(DartTexture.Width, DartTexture.Height);
            var dartTexturePosition = temp + dartSize*0.5f - dartTextureSize*0.5f;
            if (i == _mode.CurrentPlayerRound.Darts.Count)
            {
                var alpha = 0.33f*(float) (Math.Sin(XnaDartsGame.ElapsedTime*MathHelper.Pi/DartBlinkRate));
                spriteBatch.Draw(_solidDart, dartTexturePosition, Color.White*alpha);
            }

            spriteBatch.Draw(DartTexture, dartTexturePosition, Color.White);
            var numberOffset = new Vector2(_numberTextures[0].Width, _numberTextures[0].Height)*0.5f;
            return numberOffset;
        }
    }
}