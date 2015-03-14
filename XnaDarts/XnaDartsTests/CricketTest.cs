using System.Linq;
using NUnit.Framework;
using XnaDarts.Gameplay.Modes;

namespace XnaDartsTests
{
    [TestFixture]
    public class CricketTest
    {
        private CricketSegment getSegment(Cricket cricket, int segment)
        {
            return cricket.Segments.First(s => s.Segment == segment);
        }

        [Test]
        public void AllSegmentsAreClosed()
        {
            var cricket = new Cricket(2);
            Assert.IsFalse(cricket.AllSegmentsAreClosed());

            // Player 1, open 25, 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsTrue(getSegment(cricket, 25).IsOpen);
            cricket.RegisterDart(20, 3);
            Assert.IsTrue(getSegment(cricket, 20).IsOpen);
            cricket.NextPlayer();

            // Player 2, close 25, 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsFalse(getSegment(cricket, 25).IsOpen);
            cricket.RegisterDart(20, 3);
            Assert.IsFalse(getSegment(cricket, 25).IsOpen);
            cricket.NextPlayer();

            // Player 1, open 19, 18, 17
            cricket.RegisterDart(19, 3);
            Assert.IsTrue(getSegment(cricket, 19).IsOpen);
            cricket.RegisterDart(18, 3);
            Assert.IsTrue(getSegment(cricket, 18).IsOpen);
            cricket.RegisterDart(17, 3);
            Assert.IsTrue(getSegment(cricket, 17).IsOpen);
            cricket.NextPlayer();

            // Player 2, close 19, 18, 17
            cricket.RegisterDart(19, 3);
            Assert.IsFalse(getSegment(cricket, 19).IsOpen);
            cricket.RegisterDart(18, 3);
            Assert.IsFalse(getSegment(cricket, 18).IsOpen);
            cricket.RegisterDart(17, 3);
            Assert.IsFalse(getSegment(cricket, 17).IsOpen);
            cricket.NextPlayer();

            // Player 1, open 16, 15
            cricket.RegisterDart(16, 3);
            Assert.IsTrue(getSegment(cricket, 16).IsOpen);
            cricket.RegisterDart(15, 3);
            Assert.IsTrue(getSegment(cricket, 15).IsOpen);
            cricket.RegisterDart(0, 0);
            cricket.NextPlayer();

            // Player 2, close 16, 15
            cricket.RegisterDart(16, 3);
            Assert.IsFalse(getSegment(cricket, 16).IsOpen);
            cricket.RegisterDart(15, 3);
            Assert.IsFalse(getSegment(cricket, 15).IsOpen);
            cricket.RegisterDart(0, 0);

            Assert.IsTrue(cricket.AllSegmentsAreClosed());
            Assert.IsTrue(cricket.IsGameOver());
        }

        [Test]
        public void LeaderOwnsAllOpenSegments()
        {
            var cricket = new Cricket(2);
            Assert.IsFalse(cricket.LeaderOwnsAllOpenSegments());

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

            Assert.IsTrue(cricket.LeaderOwnsAllOpenSegments());
            Assert.IsTrue(cricket.IsGameOver());
        }

        [Test]
        public void Unthrow()
        {
            var cricket = new Cricket(1);
            var player1 = cricket.Players[0];

            // Player 1, open 25
            cricket.RegisterDart(25, 1);
            Assert.AreEqual(0, cricket.GetScore(player1));

            cricket.RegisterDart(25, 2);
            Assert.AreEqual(0, cricket.GetScore(player1));

            // Player 1, score 50 points
            cricket.RegisterDart(25, 2);
            Assert.AreEqual(50, cricket.GetScore(player1));

            // Player 1, unthrow, remove last 50 points
            cricket.Unthrow();
            Assert.AreEqual(0, cricket.GetScore(player1));
        }

        [Test]
        public void UnthrowTwoPlayer()
        {
            var cricket = new Cricket(2);

            // Player 1, open 25 and 20
            cricket.RegisterDart(25, 2);
            cricket.RegisterDart(25, 1);
            Assert.IsTrue(getSegment(cricket, 25).IsOpen);
            Assert.AreEqual(cricket.Players[0], getSegment(cricket, 25).Owner);

            cricket.RegisterDart(20, 3);
            Assert.IsTrue(getSegment(cricket, 20).IsOpen);
            Assert.AreEqual(cricket.Players[0], getSegment(cricket, 20).Owner);

            cricket.NextPlayer();

            // Player 2, close 20
            cricket.RegisterDart(20, 3);
            Assert.IsFalse(getSegment(cricket, 20).IsOpen);
            Assert.AreEqual(null, getSegment(cricket, 20).Owner);

            cricket.Unthrow();
            Assert.IsTrue(getSegment(cricket, 20).IsOpen);
            Assert.AreEqual(cricket.Players[0], getSegment(cricket, 20).Owner);
        }

        [Test]
        public void UnthrowMultiple()
        {
            var cricket = new Cricket(1);
            var segment25 = getSegment(cricket, 25);
            var player1 = cricket.Players[0];

            // Player 1, open 25 and score 100
            cricket.RegisterDart(25, 2);
            Assert.AreEqual(2, segment25.GetScoredMarks(player1));
            Assert.AreEqual(0, segment25.GetScore(player1));
            Assert.AreEqual(null, segment25.Owner);

            cricket.RegisterDart(25, 2);
            Assert.AreEqual(4, segment25.GetScoredMarks(player1));
            Assert.AreEqual(25, segment25.GetScore(player1));
            Assert.AreEqual(player1, segment25.Owner);

            cricket.RegisterDart(25, 2);
            Assert.AreEqual(6, segment25.GetScoredMarks(player1));
            Assert.AreEqual(75, segment25.GetScore(player1));
            Assert.AreEqual(player1, segment25.Owner);

            cricket.NextPlayer();
            cricket.Unthrow();
            Assert.AreEqual(4, segment25.GetScoredMarks(player1));
            Assert.AreEqual(25, segment25.GetScore(player1));
            Assert.AreEqual(player1, segment25.Owner);

            cricket.Unthrow();
            Assert.AreEqual(2, segment25.GetScoredMarks(player1));
            Assert.AreEqual(0, segment25.GetScore(player1));
            Assert.AreEqual(null, segment25.Owner);

            cricket.Unthrow();
            Assert.AreEqual(0, segment25.GetScoredMarks(player1));
            Assert.AreEqual(0, segment25.GetScore(player1));
            Assert.AreEqual(null, segment25.Owner);

        }

    }
}