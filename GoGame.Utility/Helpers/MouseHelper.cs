using System.Windows;

namespace GoGame.Utility.Helpers;

public static class MouseHelper
{
    private static readonly List<Point> AllPossiblePoints = new();
    
    private static List<Point> GetAllPossiblePoints()
    {
        if (AllPossiblePoints.Count != 0)
        {
            var point = new Point
            {
                X = Constants.Constants.BoardHorizontalMargin + Constants.Constants.CellSize,
                Y = Constants.Constants.BoardVerticalMargin + Constants.Constants.CellSize
            };

            if (point == AllPossiblePoints[0])
                return AllPossiblePoints;

            AllPossiblePoints.Clear();
        }
        
        for (int i = 0; i < Constants.Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.Constants.CountOfCells; j++)
            {
                var point = new Point
                {
                    X = Constants.Constants.BoardHorizontalMargin + j * Constants.Constants.CellSize,
                    Y = Constants.Constants.BoardVerticalMargin + i * Constants.Constants.CellSize
                };

                AllPossiblePoints.Add(point);
            }
        }

        return AllPossiblePoints;
    }
    
    public static Point GetNearestPositionOnBoard(this Point point)
    {
        var points = GetAllPossiblePoints();
        
        foreach (var pt in points)
        {
            double currMinLength = Math.Sqrt(
                (pt.X - point.X) *
                (pt.X - point.X) +
                (pt.Y - point.Y) *
                (pt.Y - point.Y));

            if (currMinLength <= Constants.Constants.StoneSize / 2.0)
            {
                return pt;
            }
        }

        return Constants.Constants.UndefinedPoint;
    }
}