using System.Windows;
using System.Windows.Shapes;

namespace GoGame.Models.Helpers;

public static class BoardHelper
{
    public static void UpdateStoneSize(this Ellipse ellipse)
    {
        ellipse.Width = Utility.Constants.Constants.StoneSize;
        ellipse.Height = Utility.Constants.Constants.StoneSize;
    }

    public static bool IsMouseOnBoard(this Point position)
    {
        return position.X >= Utility.Constants.Constants.BoardHorizontalMargin - Utility.Constants.Constants.StoneSize / 2.0
               && position.X <= Utility.Constants.Constants.BoardHorizontalMargin + Utility.Constants.Constants.GridLineLength + Utility.Constants.Constants.StoneSize / 2.0;
    }
}