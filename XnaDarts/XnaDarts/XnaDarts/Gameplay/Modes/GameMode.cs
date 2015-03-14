using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace XnaDarts.Gameplay.Modes
{
    public enum Direction
    {
        Asc,
        Desc
    }

    /// <summary>
    ///     This class serves as the base for all GameModes
    /// </summary>
    public abstract class GameMode
    {
        public virtual void RegisterDart(int segment, int multiplier)
        {
            //Add the dart
            var dart = new Dart(CurrentPlayer, segment, multiplier);
            CurrentPlayerRound.Darts.Add(dart);
        }

        protected virtual void RemoveDart()
        {
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                CurrentPlayerRound.Darts.RemoveAt(CurrentPlayerRound.Darts.Count - 1);
            }
        }

        public void Unthrow()
        {
            //Remove last dart of the current players throws
            if (CurrentPlayerRound.Darts.Count > 0)
            {
                RemoveDart();
            }
            //Back up one player and remove dart
            else if (CurrentPlayerIndex > 0)
            {
                CurrentPlayerIndex--;
                RemoveDart();
            }
            //Back up one round
            else if (CurrentRoundIndex > 0)
            {
                CurrentRoundIndex--;
                CurrentPlayerIndex = Players.Count - 1;
                RemoveDart();
            }
        }

        public virtual bool IsEndOfTurn()
        {
            return IsLastThrow();
        }

        public virtual bool IsGameOver()
        {
            return IsLastRound() && IsLastPlayer() && IsEndOfTurn();
        }

        #region Helper Methods

        public void NextPlayer()
        {
            if (!IsLastPlayer())
            {
                CurrentPlayerIndex++;
            }
            else
            {
                CurrentPlayerIndex = 0;
                CurrentRoundIndex++;
            }
        }

        public bool IsFirstPlayer()
        {
            return CurrentPlayerIndex == 0;
        }

        public bool IsLastPlayer()
        {
            return CurrentPlayerIndex == Players.Count - 1;
        }

        public bool IsLastRound()
        {
            return CurrentRoundIndex == MaxRounds - 1;
        }

        public bool IsFirstThrow()
        {
            return CurrentPlayerRound.Darts.Count == 0;
        }

        public bool IsLastThrow()
        {
            return CurrentPlayerRound.Darts.Count == DartsPerTurn;
        }

        #endregion

        #region Scoring

        public abstract int GetScore(Player player);

        public virtual int GetScore(Round round)
        {
            return round.GetScore();
        }

        public virtual int GetScore(Dart dart)
        {
            return dart.GetScore();
        }

        public virtual List<Player> GetLeaders()
        {
            var leaders = Players.GroupBy(GetScore).OrderBy(g => g.Key);
            if (ScoringDirection == Direction.Asc)
            {
                return leaders.First().ToList();
            }
            return leaders.Last().ToList();
        }

        #endregion

        #region Fields and Properties

        public int CurrentPlayerIndex { get; private set; }

        public int CurrentRoundIndex { get; private set; }
        public const int DartsPerTurn = 3;
        public int MaxRounds = 8;

        public abstract string Name { get; }

        public Round CurrentPlayerRound
        {
            get { return CurrentPlayer.Rounds[CurrentRoundIndex]; }
        }

        public Player CurrentPlayer
        {
            get { return Players[CurrentPlayerIndex]; }
        }

        public Direction ScoringDirection = Direction.Desc;
        public List<Player> Players = new List<Player>();

        public Color GetPlayerColor(Player player)
        {
            var index = Players.FindIndex(p => p == player);
            return XnaDartsGame.Options.PlayerColors[index%XnaDartsGame.Options.PlayerColors.Length];
        }

        #endregion

        #region Constructor

        protected GameMode(int players)
        {
            initializePlayersAndRounds(players);
        }

        private void initializePlayersAndRounds(int players)
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
    }
}