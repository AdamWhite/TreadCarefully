namespace TechTest
{
    class Program
    {
        static void Main()
        {
            var game = new Game(new GraphicsProcessor(), new Board(5), new Player());
            game.GameLoop();
        }
    }
}
