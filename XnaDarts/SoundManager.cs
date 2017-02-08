using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace XnaDarts
{
    public enum SoundCue
    {
        MenuSelect,
        MenuEnter,
        SingleBull,
        DoubleBull,
        GameStart,
        Won,
        Bust,
        NewRound,
        LastRound,
        ThrowStart,
        Single,
        Double,
        Triple,
        CricketClosed,
        MenuBack,
        Miss
    }

    public class SoundManager
    {
        private static readonly Dictionary<SoundCue, SoundEffect> LoadedSongs = new Dictionary<SoundCue, SoundEffect>();
        private readonly ContentManager _content;

        public SoundManager(ContentManager content)
        {
            _content = content;
        }

        private string getFilename(SoundCue cue)
        {
            var dir = @"Sounds\";
            switch (cue)
            {
                case SoundCue.MenuSelect:
                    return dir + "select";
                case SoundCue.MenuEnter:
                    return dir + "enter";
                case SoundCue.MenuBack:
                    return dir + "enter";
                case SoundCue.SingleBull:
                    return dir + "singlebullseye";
                case SoundCue.DoubleBull:
                    return dir + "doublebullseye";
                case SoundCue.GameStart:
                    return dir + "gamestart";
                case SoundCue.Won:
                    return dir + "applause";
                case SoundCue.Bust:
                    return dir + "bust";
                case SoundCue.LastRound:
                    return dir + "finalround";
                case SoundCue.NewRound:
                    return dir + "roundstart";
                case SoundCue.ThrowStart:
                    return dir + "throwstart";
                case SoundCue.Single:
                    return dir + "single";
                case SoundCue.Double:
                    return dir + "double";
                case SoundCue.Triple:
                    return dir + "triple";
                case SoundCue.CricketClosed:
                    return dir + "cricketclosed";
                case SoundCue.Miss:
                    return dir + "miss";
            }

            return "";
        }

        public void PlaySound(SoundCue cue)
        {
            if (!LoadedSongs.Keys.Contains(cue))
            {
                LoadedSongs.Add(cue, _content.Load<SoundEffect>(getFilename(cue)));
            }

            LoadedSongs[cue].Play(XnaDartsGame.Options.Volume, 0, 0);
        }
    }
}