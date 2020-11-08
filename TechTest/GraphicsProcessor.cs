using System;

namespace TechTest
{
    public class GraphicsProcessor : IGraphicsProcessor
    {
        public ConsoleKey GetKeyboardInput()
        {
            return Console.ReadKey(true).Key;
        }

        public void ShowText(string text)
        {
            Console.WriteLine(text);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}