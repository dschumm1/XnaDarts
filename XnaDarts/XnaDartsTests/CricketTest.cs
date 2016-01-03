using System.Linq;
using NUnit.Framework;
using XnaDarts.Gameplay.Modes;
using XnaDarts.Gameplay.Modes.Cricket;

namespace XnaDartsTests
{
    [TestFixture]
    public class CricketTest
    {
        [Test]
        public void ClosedScore()
        {
            var cricket = new Cricket(2);

            cricket.RegisterDart(20, 3);
            cricket.RegisterDart(20, 3);
            cricket.RegisterDart(20, 3);
            Assert.AreEqual(120, cricket.GetScore(cricket.Players[0]));
            cricket.NextPlayer();

            cricket.RegisterDart(20, 3);
            Assert.AreEqual(120, cricket.GetScore(cricket.Players[0]));
            Assert.IsFalse(cricket.IsSegmentOpen(20));
        }

        [Test]
        public void AllSegmentsAreClosed()
        {
            var cricket = new Cricket(2);

            // Player 1, open 25, 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsTrue(cricket.IsSegmentOpen(25));
            cricket.RegisterDart(20, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(20));
            cricket.NextPlayer();

            // Player 2, close 25, 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsFalse(cricket.IsSegmentOpen(25));
            cricket.RegisterDart(20, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(25));
            cricket.NextPlayer();

            // Player 1, open 19, 18, 17
            cricket.RegisterDart(19, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(19));
            cricket.RegisterDart(18, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(18));
            cricket.RegisterDart(17, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(17));
            cricket.NextPlayer();

            // Player 2, close 19, 18, 17
            cricket.RegisterDart(19, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(19));
            cricket.RegisterDart(18, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(18));
            cricket.RegisterDart(17, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(17));
            cricket.NextPlayer();

            // Player 1, open 16, 15
            cricket.RegisterDart(16, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(16));
            cricket.RegisterDart(15, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(15));
            cricket.RegisterDart(0, 0);
            cricket.NextPlayer();

            // Player 2, close 16, 15
            cricket.RegisterDart(16, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(16));
            cricket.RegisterDart(15, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(15));
            cricket.RegisterDart(0, 0);

            Assert.IsTrue(cricket.IsGameOver);
        }

        [Test]
        public void LeaderOwnsAllOpenSegments()
        {
            var cricket = new Cricket(2);

            // Player 1, open 25 and 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            cricket.RegisterDart(20, 3);
            cricket.NextPlayer();

            // Player 2, open 19, 18, 17
            cricket.RegisterDart(19, 3);
            cricket.RegisterDart(18, 3);
            cricket.RegisterDart(17, 3);
            cricket.NextPlayer();

            // Player 1, open 16, 15, score 20
            cricket.RegisterDart(16, 3);
            cricket.RegisterDart(15, 3);
            cricket.RegisterDart(20, 3);
            cricket.NextPlayer();

            // Player 2, miss
            cricket.RegisterDart(0, 0);
            cricket.RegisterDart(0, 0);
            cricket.RegisterDart(0, 0);
            cricket.NextPlayer();

            // Player 1, close  19, 18, 17
            cricket.RegisterDart(19, 3);
            cricket.RegisterDart(18, 3);
            cricket.RegisterDart(17, 3);

            Assert.IsTrue(cricket.IsGameOver);
        }

        [Test]
        public void UnthrowTwoPlayer()
        {
            var cricket = new Cricket(2);

            // Player 1, open 25 and 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsTrue(cricket.IsSegmentOpen(25));
            Assert.Contains(cricket.Players[0], cricket.PlayersWhoOwnsSegment(25));

            cricket.RegisterDart(20, 3);
            Assert.IsTrue(cricket.IsSegmentOpen(20));
            Assert.Contains(cricket.Players[0], cricket.PlayersWhoOwnsSegment(20));

            cricket.NextPlayer();

            // Player 2, close 20
            cricket.RegisterDart(20, 3);
            Assert.IsFalse(cricket.IsSegmentOpen(20));
            Assert.Contains(cricket.Players[0], cricket.PlayersWhoOwnsSegment(20));

            cricket.Unthrow();
            Assert.IsTrue(cricket.IsSegmentOpen(20));
            Assert.Contains(cricket.Players[0], cricket.PlayersWhoOwnsSegment(20));
        }
    }
}