using NUnit.Framework;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.ZeroOne;

namespace XnaDartsTests
{
    [TestFixture]
    public class ZeroOneTest
    {
        [Test]
        public void IsBust()
        {
            var myZeroOne = new ZeroOne(1, 20);
            myZeroOne.RegisterDart(19, 1);
            Assert.IsFalse(myZeroOne.IsBust());
            myZeroOne.RegisterDart(2, 1);
            Assert.IsTrue(myZeroOne.IsBust());
        }

        [Test]
        public void IsEndOfTurnThrownAllDarts()
        {
            var myZeroOne = new ZeroOne(1, 20);
            Assert.IsFalse(myZeroOne.IsEndOfTurn);
            myZeroOne.RegisterDart(0, 0);
            myZeroOne.RegisterDart(0, 0);
            myZeroOne.RegisterDart(0, 0);
            Assert.IsTrue(myZeroOne.IsEndOfTurn);
        }

        [Test]
        public void IsEndOfTurnBust()
        {
            var myZeroOne = new ZeroOne(1, 20);
            Assert.IsFalse(myZeroOne.IsEndOfTurn);
            myZeroOne.RegisterDart(25, 2);
            Assert.IsTrue(myZeroOne.IsEndOfTurn);
        }

        [Test]
        public void IsEndOfTurnWon()
        {
            var myZeroOne = new ZeroOne(1, 20);
            Assert.IsFalse(myZeroOne.IsEndOfTurn);
            myZeroOne.RegisterDart(20, 1);
            Assert.IsTrue(myZeroOne.IsEndOfTurn);
        }

        [Test]
        public void IsLastPlayerAndEndOfTurnAndSomeoneHasWon()
        {
            var myZeroOne = new ZeroOne(2, 20);
            Assert.IsFalse(myZeroOne.IsEndOfTurn);
            Assert.IsFalse(myZeroOne.IsGameOver);
            myZeroOne.RegisterDart(20, 1);
            Assert.IsTrue(myZeroOne.IsEndOfTurn);
            Assert.IsFalse(myZeroOne.IsGameOver);
            myZeroOne.NextPlayer();
            myZeroOne.RegisterDart(0, 0);
            Assert.IsFalse(myZeroOne.IsGameOver);
            myZeroOne.RegisterDart(0, 0);
            myZeroOne.RegisterDart(0, 0);
            Assert.IsTrue(myZeroOne.IsGameOver);
        }

        [Test]
        public void SecondRoundScore()
        {
            var myZeroOne = new ZeroOne(1, 301);
            Assert.IsFalse(myZeroOne.IsEndOfTurn);
            Assert.IsFalse(myZeroOne.IsGameOver);
            myZeroOne.RegisterDart(20, 3);
            myZeroOne.RegisterDart(20, 3);
            myZeroOne.RegisterDart(20, 3);
            Assert.IsTrue(myZeroOne.IsEndOfTurn);
            Assert.IsFalse(myZeroOne.IsGameOver);
            myZeroOne.NextPlayer();
            Assert.AreEqual(121, myZeroOne.GetScore(myZeroOne.CurrentPlayer));
        }
    }
}