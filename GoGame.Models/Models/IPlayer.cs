using System.Windows;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGame.Models.Models;

public interface IPlayer
{
    public StonesStates StoneColour { get; }
    public string Name { get; }
    public bool Resign { get; }
    
    public int SkippedCount { get; }
    public int CapturedStones { get; }
    
    public bool HasMouse { get; }
    
    public Ellipse Mouse { get; }
    Task Move();
    
    void OnResign(object sender, RoutedEventArgs e);
}