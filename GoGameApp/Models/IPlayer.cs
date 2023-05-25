using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp.Models;

public interface IPlayer
{
    public StonesStates StoneColour { get; }
    public string Name { get; }
    public bool Resign { get; }
    
    public bool HasMouse { get; }
    
    public Ellipse Mouse { get; }
    Task Move();
}