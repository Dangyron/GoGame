namespace GoGameApp;

public class Player : IPlayer
{
    public Player Enemy { get; }
    
    public bool IsCanMakeMove { get; private set; }
    public StonesColour Colour { get; }
    public string Name { get; }
    
    public Player(StonesColour colour, string name, Player enemy)
    {
        Colour = colour;
        Name = name;
        Enemy = enemy;
    }

    public void Move()
    {
        IsCanMakeMove = false;
        Enemy.IsCanMakeMove = true;
    }

    public void Resign()
    {
        
    }
}