using System.Windows.Shapes;

namespace GoGame.Utility.Models;

public interface IPlayer
{
    public StonesStates StoneColour { get; }
    public string Name { get; }
    public bool Resign { get; }
    
    public int CapturedStones { get; }
    
    public bool HasMouse { get; }
    
    public Ellipse Mouse { get; }
    Task Move();
}