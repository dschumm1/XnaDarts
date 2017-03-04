using System.Linq;
using NUnit.Framework;
using XnaDarts.Gameplay.Modes.ZeroOne;

namespace XnaDartsTests
{
    [TestFixture]
    public class ZeroOneMasterOutTests
    {
        [Test]
        public void ZeroOneBustIfBelowZero()
        {
            var zeroOne = new ZeroOne(1, 100) {IsMasterOut = true};
            zeroOne.RegisterDart(25, 2);
            zeroOne.RegisterDart(20, 3);
            Assert.IsTrue(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
        }

        [Test]
        public void ZeroOneBustIfAtOnePoint()
        {
            var zeroOne = new ZeroOne(1, 26) { IsMasterOut = true };
            zeroOne.RegisterDart(25, 1);
            Assert.IsTrue(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
        }

        [Test]
        public void ZeroOneBustIfZeroOnNonDouble()
        {
            var zeroOne = new ZeroOne(1, 20) { IsMasterOut = true };
            zeroOne.RegisterDart(20, 1);
            Assert.IsTrue(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
        }

        [Test]
        public void ZeroOnePlayer1InLeadIfCleared()
        {
            var zeroOne = new ZeroOne(2, 20) { IsMasterOut = true };
            zeroOne.RegisterDart(10, 2);
            Assert.IsFalse(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
            Assert.AreEqual(zeroOne.GetLeaders().Count, 1);
            Assert.AreEqual(zeroOne.GetLeaders().First(), zeroOne.Players.First());
        }

        [Test]
        public void ZeroOneIsGameOverIfBustInLastRound()
        {
            var zeroOne = new ZeroOne(1, 20) { IsMasterOut = true, MaxRounds = 1 };
            zeroOne.RegisterDart(20, 1);
            Assert.IsTrue(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
            Assert.IsTrue(zeroOne.IsGameOver);
        }

        [Test]
        public void ZeroOneIsGameOverIfFirstPlayerCloseAndLastPlayerBust()
        {
            var zeroOne = new ZeroOne(2, 40) { IsMasterOut = true, MaxRounds = 2 };
            zeroOne.RegisterDart(20, 2);
            Assert.IsFalse(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
            Assert.IsTrue(zeroOne.IsAtZero(zeroOne.CurrentPlayer));
            Assert.IsFalse(zeroOne.IsGameOver);
            zeroOne.NextPlayer();
            zeroOne.RegisterDart(20, 3);
            Assert.IsTrue(zeroOne.IsAtZero(zeroOne.Players[0]));
            Assert.IsTrue(zeroOne.IsEndOfTurn);
            Assert.IsTrue(zeroOne.IsPlayerBustAtCurrentRound(zeroOne.CurrentPlayer));
            Assert.IsFalse(zeroOne.IsAtZero(zeroOne.CurrentPlayer));
            Assert.IsTrue(zeroOne.IsGameOver);
        }
    }
}
