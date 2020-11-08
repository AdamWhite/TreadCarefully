using System;

namespace TechTest
{
    public class Board : IBoard
    {
        private bool[,] _visited;
        private Tile[,] _bombs;
        private int _size;

        public Board(int size)
        {
            var bombLocations = GenerateBombs(size);
            _bombs = bombLocations;
            SetupBoard(bombLocations, size);
        }

        public Board(Tile[,] bombLocations, int size)
        {
            SetupBoard(bombLocations, size);
        }

        public bool DidBombExplodeAtLocation(int x, int y)
        {
            if (_bombs[x, y] == Tile.Bomb)
            {
                _bombs[x, y] = Tile.Exploded;
                return true;
            }

            return false;
        }

        public bool Move(IPlayer player, Direction direction)
        {
            var currentX = player.X;
            var currentY = player.Y;

            switch (direction)
            {
                case Direction.Up:
                {
                    if (++currentY >= _size)
                    {
                        return false;
                    }

                    player.Y++;

                    break;
                }

                case Direction.Down:
                {
                    if (--currentY < 0)
                    {
                        return false;
                    }

                    player.Y--;

                    break;
                }

                case Direction.Left:
                {
                    if (--currentX < 0)
                    {
                        return false;
                    }

                    player.X--;

                    break;
                }

                case Direction.Right:
                {
                    if (++currentX >= _size)
                    {
                        return false;
                    }

                    player.X++;
                    
                    break;
                }
            }

            return true;
        }

        public bool HasPlayerVisited(IPlayer player)
        {
            return _visited[player.X, player.Y];
        }

        public void VisitTile(int x, int y)
        {
            _visited[x, y] = true;
        }

        private void ResetVisited()
        {
            for (var i = 0; i < _size; i++)
            {
                for (var y = 0; y < _size; y++)
                {
                    _visited[i, y] = false;
                }
            }
        }

        private Tile[,] GenerateBombs(int size)
        {
            var bombs = new Tile[size, size];
            Random random = new Random();

            var mines = Math.Round((size * size) * .25, 0);
            var minesPlaced = 0;

            while (minesPlaced < mines)
            {
                var x = random.Next(0, size);
                var y = random.Next(0, size);

                bombs[x, y] = Tile.Bomb;

                minesPlaced++;
            }

            return bombs;
        }

        private void SetupBoard(Tile[,] bombLocations, int size)
        {
            _bombs = bombLocations;
            _size = size;
            _visited = new bool[size, size];
            ResetVisited();
            VisitTile(0, 0);
            _bombs[0, 0] = Tile.Empty;
        }
    }
}