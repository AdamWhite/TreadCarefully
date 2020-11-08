namespace TechTest
{
    public interface IGame
    {
        void GameLoop();
        void ShowMenu();
        void ShowEndScreen();
        Direction GetInput();
        void Play(Direction direction);
    }
}