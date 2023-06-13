using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Utility.Constants;

namespace GoGame.Models.Helpers;

public static class BoardHelper
{
    public static void UpdateStoneSize(this Ellipse ellipse)
    {
        ellipse.Width = Constants.StoneSize;
        ellipse.Height = Constants.StoneSize;
    }

    public static bool IsMouseOnBoard(this Point position)
    {
        return position.X >= Constants.BoardHorizontalMargin - Constants.StoneSize / 2.0
               && position.X <= Constants.BoardHorizontalMargin + Constants.GridLineLength + Constants.StoneSize / 2.0;
    }

    public static Ellipse NewStar()
    {
        return new Ellipse()
        {
            Width = Constants.BoardStarSize,
            Height = Constants.BoardStarSize,
            Fill = Constants.BoardStrokeColour
        };
    }
}