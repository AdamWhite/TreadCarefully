using System;
using NUnit.Framework;

namespace TechTest.Tests
{
    [TestFixture]
    public class BoardTests
    {

        [Test]
        [TestCase(0, Direction.Left)]
        [TestCase(5, Direction.Right)]
        [TestCase(5, Direction.Up)]
        [TestCase(0, Direction.Down)]

        public void Player_CanNotMoveOutOfBounds(int location, Direction direction)
        {
            var player = new Player();
            player.X = location;
            player.Y = location;

            var board = new Board(5);

            var moved = board.Move(player, direction);

            Assert.IsFalse(moved);
        }

        [Test]
        [TestCase(1, Direction.Left)]
        [TestCase(3, Direction.Right)]
        [TestCase(2, Direction.Up)]
        [TestCase(3, Direction.Down)]

        public void Player_CanMoveWithinBounds(int location, Direction direction)
        {
            var player = new Player();
            player.X = location;
            player.Y = location;

            var board = new Board(5);

            var moved = board.Move(player, direction);

            Assert.IsTrue(moved);
        }

        [Test]
        public void Player_CanMoveWithinBounds()
        {
            var player = new Player();
            player.X = 0;
            player.Y = 0;

            var board = new Board(5);

            var moved = board.Move(player, Direction.Right);

            Assert.IsTrue(moved);
            Assert.AreEqual(player.X, 1);
        }


        [Test]
        public void Board_BombAtLocationReturnsTrueIfBombExists()
        {
            int size = 2;
            var bombLocations = new Tile[size, size];
            bombLocations[1, 0] = Tile.Bomb;

            var board = new Board(bombLocations, size);
            var didBombExplode = board.DidBombExplodeAtLocation(1, 0);

            Assert.IsTrue(didBombExplode);
        }

        [Test]
        public void Board_BombAtLocationReturnsFalseIfBombExploded()
        {
            int size = 2;
            var bombLocations = new Tile[size, size];
            bombLocations[1, 0] = Tile.Bomb;

            var board = new Board(bombLocations, size);
            board.DidBombExplodeAtLocation(1, 0);
            var didBombExplode = board.DidBombExplodeAtLocation(1, 0);

            Assert.IsFalse(didBombExplode);
        }

        [Test]
        public void Board_WhenPlayerHasNotVistedTileReturnsFalse()
        {
            int size = 2;
            var bombLocations = new Tile[size, size];
            bombLocations[1, 0] = Tile.Bomb;

            var player = new Player(1,1,3);
            var board = new Board(bombLocations, size);
            var visited = board.HasPlayerVisited(player);

            Assert.IsFalse(visited);
        }

        [Test]
        public void Board_WhenPlayerHasVistedTileReturnsTrue()
        {
            int size = 2;
            var bombLocations = new Tile[size, size];
            bombLocations[1, 0] = Tile.Bomb;

            var player = new Player(1, 1, 3);
            var board = new Board(bombLocations, size);
            board.VisitTile(player.X, player.Y);
            var visited = board.HasPlayerVisited(player);

            Assert.IsTrue(visited);
        }

        [Test]
        public void thing()
        {

        }
    }
}
