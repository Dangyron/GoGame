using System.Windows.Shapes;

namespace GoGame.Utility.Helpers;

public static class BoardHelper
{
    public static void UpdateStoneSize(this Ellipse ellipse)
    {
        ellipse.Width = Constants.Constants.StoneSize;
        ellipse.Height = Constants.Constants.StoneSize;
    }

    public static void UpdateStoneColour(this Ellipse ellipse)
    {
        
    }
}