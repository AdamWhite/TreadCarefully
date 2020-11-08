using System;
using Moq;
using NUnit.Framework;

namespace TechTest.Tests
{
    [TestFixture]
    public class GameTests
    {
        private Mock<IGraphicsProcessor> _graphicsProcessormock;
        private Mock<IBoard> _board;
        private Mock<IPlayer> _player;

        [SetUp]
        public void SetUp()
        {
            _graphicsProcessormock = new Mock<IGraphicsProcessor>();
            _board = new Mock<IBoard>();
            _player = new Mock<IPlayer>();
        }

        [Test]
        public void Game_WhenShowEndScreenCalled_TextIsShownAndUserInputRequested()
        {
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.ShowEndScreen();

            _graphicsProcessormock.Verify(x => x.ShowText(It.IsAny<string>()), Times.Once);
            _graphicsProcessormock.Verify(x => x.GetKeyboardInput(),Times.Once);
        }

        [Test]
        public void Game_WhenShowMenuCalled_ScreenIsClearedAndTextIsShown()
        {
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.ShowMenu();

            _graphicsProcessormock.Verify(x => x.ShowText(It.IsAny<string>()), Times.Exactly(3));
            _graphicsProcessormock.Verify(x => x.ClearScreen(), Times.Once);
        }

        [TestCase(ConsoleKey.UpArrow, Direction.Up)]
        [TestCase(ConsoleKey.DownArrow, Direction.Down)]
        [TestCase(ConsoleKey.LeftArrow, Direction.Left)]
        [TestCase(ConsoleKey.RightArrow, Direction.Right)]
        public void Game_WhenKeyPressed_DirectionMatches(ConsoleKey key, Direction direction)
        {
            _graphicsProcessormock.Setup(x => x.GetKeyboardInput()).Returns(key);
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            var directionSelected = game.GetInput();

            Assert.AreEqual(direction, directionSelected);
        }

        [Test]
        public void Game_WhenInvalidKeyPressedErrorIsShownToScreen_UntilCorrectKeyPressed()
        {
            _graphicsProcessormock.SetupSequence(x => x.GetKeyboardInput()).Returns(ConsoleKey.A).Returns(ConsoleKey.UpArrow);
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            var directionSelected = game.GetInput();

            _graphicsProcessormock.Verify(x => x.ShowText(It.IsAny<string>()), Times.Once);
        }
        
        [Test]
        public void Game_WhenPlayIsCalledAndPlayerCannotMove_MessageSet()
        {
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(false);
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.Play(Direction.Down);

            _player.VerifySet(x => x.Message = It.IsAny<string>(), Times.Once());
        }

        [Test]
        public void Game_WhenPlayIsCalledAndPlayerVistsLocationNotVisted_ThenVisitTileIsCalledAndMovementsIncreased()
        {
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(true);
            _board.Setup(x => x.HasPlayerVisited(It.IsAny<IPlayer>())).Returns(false);

            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.Play(Direction.Down);

            _board.Verify(x => x.VisitTile(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _player.VerifySet(movements => movements.MovementsMade = 1, Times.Once());

        }

        [Test]
        public void Game_WhenPlayIsCalledAndPlayerMoves_DidBombExplodeAtLocationIsCalled()
        {
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(true);
            _board.Setup(x => x.HasPlayerVisited(It.IsAny<IPlayer>())).Returns(true);

            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.Play(Direction.Down);

            _board.Verify(x => x.DidBombExplodeAtLocation(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Game_WhenPlayIsCalledAndPlayerStepsOnAnUnexplodedBomb_PlayerLosesALife()
        {
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(true);
            _board.Setup(x => x.HasPlayerVisited(It.IsAny<IPlayer>())).Returns(true);
            _board.Setup(x => x.DidBombExplodeAtLocation(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.Play(Direction.Down);

            _player.Verify(x => x.RemoveLife(), Times.Once);
        }

        [Test]
        public void Game_WhenPlayIsCalledAndPlayerStepsOnAnUnexplodedBombThenStepsOntoTheBombAgain_PlayerLosesOnlyOneLife()
        {
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(true);
            _board.Setup(x => x.HasPlayerVisited(It.IsAny<IPlayer>())).Returns(true);
            _board.SetupSequence(x => x.DidBombExplodeAtLocation(It.IsAny<int>(), It.IsAny<int>())).Returns(true).Returns(false);

            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);

            game.Play(Direction.Down);
            game.Play(Direction.Up);

            _player.Verify(x => x.RemoveLife(), Times.Once);
        }

        [Test]
        public void Game_WhenGameLoopIsCalledAndGameStateIsEnd_PlayerDoesNotMove()
        {
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);
            game.GameState = GameState.End;
            game.GameLoop();

            _board.Verify(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>()), Times.Never);
        }

        [Test]
        public void Game_WhenGameLoopIsCalledAndGameStateIsRunningAndPlayerIsNotAlive_GameStateChangesToEnd()
        {
            _graphicsProcessormock.Setup(x => x.GetKeyboardInput()).Returns(ConsoleKey.UpArrow);
            _board.Setup(x => x.Move(It.IsAny<IPlayer>(), It.IsAny<Direction>())).Returns(true);
            _board.Setup(x => x.HasPlayerVisited(It.IsAny<IPlayer>())).Returns(true);
            _board.Setup(x => x.DidBombExplodeAtLocation(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            var game = new Game(_graphicsProcessormock.Object, _board.Object, _player.Object);
            game.GameState = GameState.Running;
            _player.Setup(x => x.IsPlayerAlive()).Returns(false);
            game.GameLoop();

            Assert.AreEqual(GameState.End, game.GameState);
        }
    }
}