namespace GoGameApp;

public interface IPlayer
{
    public Player Enemy { get; }
    public bool IsCanMakeMove { get; }
    public StonesColour Colour { get; }
    public string Name { get; }
    void Move();
    void Resign();
}