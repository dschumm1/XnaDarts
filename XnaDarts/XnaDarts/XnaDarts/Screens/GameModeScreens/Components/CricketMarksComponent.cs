using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaDarts.Gameplay.Modes;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameModeScreens.Components
{
    public class CricketMarksComponent : IDrawableGameComponent
    {
        private const float Scale = 0.8f;
        private Texture2D _bullTexture;
        private Texture2D _closedTexture;
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

            for (var i = 0; i < 6; i++)
            {
                _numberTextures[i] =
                    content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\" + (i + 15));
            }

            _bullTexture = content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\Bull");
            _closedTexture =
                content.Load<Texture2D>(@"Images\" + XnaDartsGame.Options.Theme + @"\" + @"CricketNumbers\Closed");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(XnaDartsGame.Viewport.Width*0.5f, XnaDartsGame.Viewport.Height*0.1f);
            const float scale = 0.6f;

            var tempColor = Color.Black;
            tempColor.A = 80;

            //Draw panels for marks
            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle((int) (position.X - _markTexture[0].Width*1.5f*scale),
                    (int) (position.Y - _markTexture[0].Height*0.5f*scale),
                    (int) (_markTexture[0].Width*scale),
                    (int) (_markTexture[0].Height*7*scale)), tempColor);

            spriteBatch.Draw(ScreenManager.BlankTexture,
                new Rectangle((int) (position.X + _markTexture[0].Width*0.5f*scale),
                    (int) (position.Y - _markTexture[0].Height*0.5f*scale),
                    (int) (_markTexture[0].Width*scale),
                    (int) (_markTexture[0].Height*7*scale)), tempColor);


            //Draw marks
            for (var i = 0; i < _mode.Segments.Count; i++)
            {
                drawMarks(spriteBatch, position, _mode.Segments[i]);
                position.Y += _markTexture[0].Height*scale;
            }
        }

        private void drawMarks(SpriteBatch spriteBatch, Vector2 position, CricketSegment segment)
        {
            var font = ScreenManager.Trebuchet24;
            var temp = position - new Vector2(_markTexture[0].Width*1.5f*Scale, _markTexture[0].Height*0.5f*Scale);
            Vector2 offset;
            Color segmentColor;

            foreach (var player in _mode.Players)
            {
                //draw marks
                var marks = segment.GetScoredMarks(player);

                segmentColor = Color.White;

                if (!segment.IsOpen)
                {
                    segmentColor = Color.White*0.33f;
                }

                if (marks > 0)
                {
                    spriteBatch.Draw(_markTexture[Math.Min(marks, 3)], temp, null, segmentColor, 0, Vector2.Zero, Scale,
                        SpriteEffects.None, 0);
                }

                var center = new Vector2(_markTexture[0].Width, _markTexture[0].Height)*Scale*0.5f;
                offset = font.MeasureString(marks.ToString())*0.5f;

                if (XnaDartsGame.Options.Debug)
                {
                    spriteBatch.DrawString(font, marks.ToString(), temp + center - offset + Vector2.One,
                        Color.Black);
                    spriteBatch.DrawString(font, marks.ToString(), temp + center - offset, Color.White);
                }

                temp.X += _markTexture[0].Width*Scale*2f;
            }

            if (XnaDartsGame.Options.Debug)
            {
                offset = font.MeasureString(segment.ToString())*0.5f;
                spriteBatch.DrawString(font, segment.ToString(), position - offset + Vector2.One, Color.Black);
                spriteBatch.DrawString(font, segment.ToString(), position - offset, Color.White);
            }

            offset = new Vector2(_numberTextures[0].Width, _numberTextures[0].Height)*0.5f;
            Texture2D tex;

            if (segment.Segment != 25)
            {
                tex = _numberTextures[segment.Segment - 15];
            }
            else
            {
                tex = _bullTexture;
            }

            segmentColor = Color.White;

            if (!segment.IsOpen)
            {
                segmentColor = Color.White*0.33f;
            }

            spriteBatch.Draw(tex, position - offset, segmentColor);

            if (!segment.IsOpen)
            {
                offset = new Vector2(_closedTexture.Width, _closedTexture.Height)*0.5f;
                spriteBatch.Draw(_closedTexture, position - offset, Color.White);
            }
        }
    }
}