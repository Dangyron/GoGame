using System.Windows.Shapes;
using GoGame.Models.Helpers;
using GoGame.Utility;

namespace GoGame.Models.Models;

public readonly struct Stone
{
    public StonesStates StoneStates { get; }
    
    public Ellipse Imagination { get; }
    public Stone(StonesStates stoneStates)
    {
        StoneStates = stoneStates;
        Imagination = new Ellipse
        {
            Width = Utility.Constants.Constants.StoneSize,
            Height = Utility.Constants.Constants.StoneSize,
            Fill = StoneStates.ConvertStonesColourToBrush()
        };
    }
}