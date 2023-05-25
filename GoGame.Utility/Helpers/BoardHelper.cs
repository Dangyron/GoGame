using System.Windows;
using System.Windows.Shapes;

namespace GoGame.Utility.Helpers;

public static class BoardHelper
{
    public static void UpdateStoneSize(this Ellipse ellipse)
    {
        ellipse.Width = Constants.Constants.StoneSize;
        ellipse.Height = Constants.Constants.StoneSize;
    }

    public static bool IsMouseOnBoard(this Point position)
    {
        return position.X >= Constants.Constants.BoardHorizontalMargin - Constants.Constants.StoneSize / 2
               && position.X <= Constants.Constants.BoardHorizontalMargin + Constants.Constants.GridLineLength + Constants.Constants.StoneSize / 2;
    }
}