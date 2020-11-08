using NUnit.Framework;

namespace TechTest.Tests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        [TestCase(0, 0, "A0")]
        [TestCase(2, 0, "C0")]
        [TestCase(4, 0, "E0")]
        [TestCase(2, 2, "C2")]
        [TestCase(5, 5, "F5")]
        public void Player_StartsChessLocation(int x, int y, string chessLocation)
        {
            var player = new Player(0,0,3);
            player.X = x;
            player.Y = y;

            Assert.AreEqual(chessLocation, player.GetLocation());
        }

        [Test]
        public void Player_Has3LivesLosesOne_IsPlayerAlive_True()
        {
            var player = new Player(0,0,3);

            player.RemoveLife();

            Assert.IsTrue(player.IsPlayerAlive());
        }

        [Test]
        public void Player_Has3LivesLoses3_IsPlayerAlive_False()
        {
            var player = new Player(0, 0, 3);

            player.RemoveLife();
            player.RemoveLife();
            player.RemoveLife();

            Assert.IsFalse(player.IsPlayerAlive());
        }
    }
}