using NUnit.Framework;
using XnaDarts.Gameplay.Modes.CountUp;

namespace XnaDartsTests
{
    [TestFixture]
    public class GameModeTest
    {
        [Test]
        public void TestNextPlayer()
        {
            var countUp = new CountUp(3);
            Assert.AreEqual(0, countUp.CurrentPlayerIndex);
            Assert.AreEqual(0, countUp.CurrentRoundIndex);
            countUp.NextPlayer();
            Assert.AreEqual(1, countUp.CurrentPlayerIndex);
            Assert.AreEqual(0, countUp.CurrentRoundIndex);
            countUp.NextPlayer();
            Assert.AreEqual(2, countUp.CurrentPlayerIndex);
            Assert.AreEqual(0, countUp.CurrentRoundIndex);
            countUp.NextPlayer();
            Assert.AreEqual(0, countUp.CurrentPlayerIndex);
            Assert.AreEqual(1, countUp.CurrentRoundIndex);
            countUp.NextPlayer();
            Assert.AreEqual(1, countUp.CurrentPlayerIndex);
            Assert.AreEqual(1, countUp.CurrentRoundIndex);
        }
    }
}