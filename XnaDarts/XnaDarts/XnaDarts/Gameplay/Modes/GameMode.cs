using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaDarts.Gameplay.Modes
{
    public abstract class GameMode
    {
        public abstract string Name { get; }

        public void RegisterDart(int segment, int multiplier)
        {
            var dart = new Dart(CurrentPlayer, segment, multiplier);
            CurrentPlayerRound.Darts.Add(dart);
        }

        private void _removeDart()
        {
            CurrentPlayerRound.Darts.RemoveAt(CurrentPlayerRound.Darts.Count - 1);
        }

        public void Unthrow()
        {
            //Remove last dart of the current players throws
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                _removeDart();
            }
            //Back up one player and remove dart
            else if (CurrentPlayerIndex > 0)
            {
                CurrentPlayerIndex--;
                _removeDart();
            }
            //Back up one round
            else if (CurrentRoundIndex > 0)
            {
                CurrentRoundIndex--;
                CurrentPlayerIndex = Players.Count - 1;
                _removeDart();
            }
        }

        public void NextPlayer()
        {
            if (!IsLastPlayer)
            {
                CurrentPlayerIndex++;
            }
            else
            {
                CurrentPlayerIndex = 0;
                CurrentRoundIndex++;
            }
        }

        #region Constructor

        public GameMode(int players)
        {
            _initializePlayersAndRounds(players);
        }

        private void _initializePlayersAndRounds(int players)
        {
            for (var i = 0; i < players; i++)
            {
                var p = new Player("Player " + (i + 1));
                Players.Add(p);

                for (var j = 0; j < MaxRounds; j++)
                {
                    Players[i].Rounds.Add(new Round());
                }
            }
        }

        #endregion

        #region Fields and Properties

        public virtual int GetScore(Player player)
        {
            return player.GetScore();
        }

        public virtual List<Player> GetLeaders()
        {
            var leaders = Players.GroupBy(GetScore).OrderBy(group => group.Key);
            return leaders.Last().ToList();
        }

        public virtual bool IsGameOver
        {
            get { return IsLastRound && IsLastPlayer && IsLastThrow; }
        }

        public virtual bool IsEndOfTurn
        {
            get { return IsLastThrow; }
        }

        public bool IsFirstPlayer
        {
            get { return CurrentPlayerIndex == 0; }
        }

        public bool IsLastPlayer
        {
            get { return CurrentPlayerIndex == Players.Count - 1; }
        }

        public bool IsLastRound
        {
            get { return CurrentRoundIndex == MaxRounds - 1; }
        }

        public bool IsFirstThrow
        {
            get { return CurrentPlayerRound.Darts.Count == 0; }
        }

        public bool IsLastThrow
        {
            get { return CurrentPlayerRound.Darts.Count == DartsPerTurn; }
        }


        public int CurrentPlayerIndex { get; private set; }

        public int CurrentRoundIndex { get; private set; }
        public const int DartsPerTurn = 3;
        public int MaxRounds = 8;

        public Round CurrentPlayerRound
        {
            get { return CurrentPlayer.Rounds[CurrentRoundIndex]; }
        }

        public Player CurrentPlayer
        {
            get { return Players[CurrentPlayerIndex]; }
        }

        public List<Player> Players = new List<Player>();

        public Color GetPlayerColor(Player player)
        {
            var index = Players.FindIndex(p => p == player);
            return XnaDartsGame.Options.PlayerColors[index%XnaDartsGame.Options.PlayerColors.Length];
        }

        #endregion
    }
}