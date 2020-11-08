using System;

namespace TechTest
{
    public interface IGraphicsProcessor
    {
        ConsoleKey GetKeyboardInput();
        void ShowText(string text);
        void ClearScreen();
    }
}