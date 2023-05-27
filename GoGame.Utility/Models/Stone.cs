using System.Windows.Shapes;
using GoGame.Utility.Helpers;

namespace GoGame.Utility.Models;

public readonly struct Stone
{
    public StonesStates StoneStates { get; }
    
    public Ellipse Imagination { get; }
    public Stone(StonesStates stoneStates)
    {
        StoneStates = stoneStates;
        Imagination = new Ellipse
        {
            Width = Constants.Constants.StoneSize,
            Height = Constants.Constants.StoneSize,
            Fill = StoneStates.ConvertStonesColourToBrush()
        };
    }
}