using System.Windows;
using System.Windows.Shapes;
using GoGame.Models.Helpers;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Models;

public readonly struct Stone
{
    public StonesStates StoneStates { get; }

    public Ellipse Imagination { get; }
    public Stone(StonesStates stoneStates)
    {
        StoneStates = stoneStates;
        Imagination = stoneStates.ConvertStonesColourToEllipse();
    }
}