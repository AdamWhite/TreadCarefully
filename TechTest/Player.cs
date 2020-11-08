namespace TechTest
{
    public class Player : IPlayer
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int MovementsMade { get; set; }
        public int Lives { get; private set; }
        public string Message { get; set; }

        public Player()
        {
            X = 0;
            Y = 0;
            Lives = 3;
        }

        public Player(int x, int y, int lives)
        {
            X = x;
            Y = y;
            Lives = lives;
        }

        public bool IsPlayerAlive()
        {
            return Lives > 0;
        }

        public int RemoveLife()
        {
            return --Lives;
        }
        
        public string GetLocation()
        {
            int AAsASCII = 65;

            return $"{char.ConvertFromUtf32(AAsASCII+ X)}{Y}";
        }
    }
}