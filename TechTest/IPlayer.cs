namespace TechTest
{
    public interface IPlayer
    {
        bool IsPlayerAlive();
        int RemoveLife();
        string GetLocation();

        int Lives { get; }
        int MovementsMade { get; set; }
        string Message { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }
}