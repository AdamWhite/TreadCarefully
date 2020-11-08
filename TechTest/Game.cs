using System;

namespace TechTest
{
    public class Game : IGame
    {
        public IBoard Board { get; }
        public IPlayer Player { get; }

        public GameState GameState { get; set; }

        private readonly IGraphicsProcessor _graphicsProcessor;

        public Game(IGraphicsProcessor graphicsProcessor, IBoard board, IPlayer player)
        {
            _graphicsProcessor = graphicsProcessor;
            Board = board;
            Player = player;

            GameState = GameState.Running;
        }

        public void GameLoop()
        {
            while (GameState != GameState.End)
            {
                ShowMenu();

                ShowPlayerMessage();
                ClearPlayerMessage();

                var chosenDirection = GetInput();
                Play(chosenDirection);

                if (!Player.IsPlayerAlive())
                {
                    GameState = GameState.End;
                    ShowPlayerMessage();
                    ShowEndScreen();
                }
            }
        }

        public void ShowMenu()
        {
            string menu = $"Lives: {Player.Lives} - Moves Taken: {Player.MovementsMade}";
            string location = $"You are at location: {Player.GetLocation()}";
            string instructions = "Use the arrow keys to move";

            _graphicsProcessor.ClearScreen();
            _graphicsProcessor.ShowText(menu);
            _graphicsProcessor.ShowText(location);
            _graphicsProcessor.ShowText(instructions);
        }

        public void ShowEndScreen()
        {
           _graphicsProcessor.ShowText("Press any key to exit!");
           _graphicsProcessor.GetKeyboardInput();
        }

        public Direction GetInput()
        {
            while (true)
            {
                var key = _graphicsProcessor.GetKeyboardInput();

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    {
                        return Direction.Up;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        return Direction.Down;
                    }
                    case ConsoleKey.LeftArrow:
                    {
                        return Direction.Left;
                    }
                    case ConsoleKey.RightArrow:
                    {
                        return Direction.Right;
                    }
                    default:
                    {
                        _graphicsProcessor.ShowText("Please use the arrow keys to move.");
                        break;
                    }
                }
            }
        }

        public void Play(Direction direction)
        {
            if (!Board.Move(Player, direction))
            { 
                Player.Message = "*phew* I almost fell off the edge of the board!";
                return;
            }

            if (!Board.HasPlayerVisited(Player))
            {
                Board.VisitTile(Player.X, Player.Y);
                Player.MovementsMade++;
            }

            if (Board.DidBombExplodeAtLocation(Player.X, Player.Y))
            {
                Player.Message = "*ahhh* *boooom* So erm... there was a bomb.... Ouch!";
                Player.RemoveLife();
            }

            if (!Player.IsPlayerAlive())
            {
                Player.Message += "\nOh no! I'm a Ghost.";
            }
        }

        private void ShowPlayerMessage()
        {
            _graphicsProcessor.ShowText(Player.Message);
        }

        private void ClearPlayerMessage()
        {
            Player.Message = string.Empty;
        }
    }
}