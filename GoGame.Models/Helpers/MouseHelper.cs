using System.Windows;
using GoGame.Utility.Constants;

namespace GoGame.Models.Helpers;

public static class MouseHelper
{
    private static readonly List<Point> AllPossiblePoints = new();
    
    private static List<Point> GetAllPossiblePoints()
    {
        if (AllPossiblePoints.Count != 0)
        {
            var point = new Point
            {
                X = Constants.BoardHorizontalMargin + Constants.CellSize,
                Y = Constants.BoardVerticalMargin + Constants.CellSize
            };

            if (point == AllPossiblePoints[0])
                return AllPossiblePoints;

            AllPossiblePoints.Clear();
        }
        
        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                var point = new Point
                {
                    X = Constants.BoardHorizontalMargin + j * Constants.CellSize,
                    Y = Constants.BoardVerticalMargin + i * Constants.CellSize
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

            if (currMinLength <= Constants.StoneSize / 2.0)
            {
                return pt;
            }
        }

        return Constants.UndefinedPoint;
    }
}