using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using XnaDarts.Gameplay;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.GameScreens
{
    public enum AwardCue
    {
        LowTon,
        HatTrick,
        ThreeInTheBlack,
        HighTon,
        ThreeInABed,
        TonEighty,
        WhiteHorse
    }

    public class AwardScreen : GameScreen
    {
        private bool _loaded;
        private readonly Dictionary<AwardCue, Video> _awards = new Dictionary<AwardCue, Video>();
        private readonly VideoPlayer _videoPlayer;
        private ContentManager _content;

        public AwardScreen()
        {
            _videoPlayer = new VideoPlayer();
        }


        public override void LoadContent()
        {
            if (_content == null)
            {
                _content = new ContentManager(XnaDartsGame.ScreenManager.Game.Services, @"Content");
            }

            if (!_loaded)
            {
                _awards.Add(AwardCue.HatTrick, _content.Load<Video>(@"Awards\HatTrick"));
                _awards.Add(AwardCue.HighTon, _content.Load<Video>(@"Awards\HighTon"));
                _awards.Add(AwardCue.LowTon, _content.Load<Video>(@"Awards\LowTon"));
                _awards.Add(AwardCue.ThreeInABed, _content.Load<Video>(@"Awards\ThreeInABed"));
                _awards.Add(AwardCue.ThreeInTheBlack, _content.Load<Video>(@"Awards\ThreeInTheBlack"));
                _awards.Add(AwardCue.TonEighty, _content.Load<Video>(@"Awards\TonEighty"));
                _awards.Add(AwardCue.WhiteHorse, _content.Load<Video>(@"Awards\WhiteHorse"));
                _loaded = true;
            }
        }

        private void play(AwardCue cue)
        {
            if (!_awards.Keys.Contains(cue))
            {
                throw new Exception("Award Error");
            }

            _videoPlayer.Volume = XnaDartsGame.Options.Volume;

            _videoPlayer.Play(_awards[cue]);
            XnaDartsGame.ScreenManager.AddScreen(this);
        }

        public void Stop()
        {
            _videoPlayer.Stop();

            XnaDartsGame.ScreenManager.RemoveScreen(this);
        }

        /// <summary>
        ///     Checks if any conditions for the awards are met and plays the award video
        /// </summary>
        public void PlayAwards(Round round)
        {
            if (!XnaDartsGame.Options.PlayAwards)
            {
                return;
            }

            if (isTonEighty(round))
            {
                play(AwardCue.TonEighty);
            }
            else if (isThreeInTheBlack(round))
            {
                //Play award three in the black
                play(AwardCue.ThreeInTheBlack);
            }
            else if (isHatTrick(round))
            {
                //Play award hattrick!
                play(AwardCue.HatTrick);
            }
            else if (isHighTon(round))
            {
                play(AwardCue.HighTon);
            }
            else if (isLowTon(round))
            {
                //Play award low ton!
                play(AwardCue.LowTon);
            }
            else if (isThreeInABed(round))
            {
                play(AwardCue.ThreeInABed);
            }
        }

        private bool isThreeInABed(Round round)
        {
            return round.Darts.All(x =>
                x.Segment != 0 &&
                x.Multiplier != 1 &&
                x.Segment == round.Darts[0].Segment &&
                x.Multiplier == round.Darts[0].Multiplier);
        }

        private bool isLowTon(Round round)
        {
            return round.GetScore() >= 100;
        }

        private bool isHighTon(Round round)
        {
            return round.GetScore() > 150;
        }

        private bool isHatTrick(Round round)
        {
            return round.Darts.TrueForAll(dart => dart.Segment == 25);
        }

        private bool isTonEighty(Round round)
        {
            return round.GetScore() == 180;
        }

        private bool isThreeInTheBlack(Round round)
        {
            return round.Darts.TrueForAll(
                dart => dart.Segment == 25 &&
                        dart.Multiplier == 2);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (inputState.MenuEnter || inputState.MenuCancel)
            {
                Stop();
            }
        }

        public override void Update(GameTime gameTime, bool isCoveredByOtherScreen)
        {
            base.Update(gameTime, isCoveredByOtherScreen);

            if (_videoPlayer.PlayPosition.Equals(_videoPlayer.Video.Duration))
            {
                Stop();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            if (_videoPlayer.State == MediaState.Playing)
            {
                spriteBatch.Draw(_videoPlayer.GetTexture(),
                    new Rectangle(0, 0, XnaDartsGame.Viewport.Width, XnaDartsGame.Viewport.Height), Color.White);
            }
            spriteBatch.End();
        }
    }
}