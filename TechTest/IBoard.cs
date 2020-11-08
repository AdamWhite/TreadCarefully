namespace TechTest
{
    public interface IBoard
    {
        bool DidBombExplodeAtLocation(int x, int y);
        bool Move(IPlayer player, Direction direction);
        bool HasPlayerVisited(IPlayer player);
        void VisitTile(int x, int y);
    }
}