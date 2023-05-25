using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp.Models;

public readonly struct Stone
{
    public StonesStates StoneStates { get; }
    
    public Ellipse Imagination { get; }
    public Stone(StonesStates stoneStates)
    {
        StoneStates = stoneStates;
        Imagination = new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = StoneStates.ConvertStonesColourToBrush()
        };
    }
}