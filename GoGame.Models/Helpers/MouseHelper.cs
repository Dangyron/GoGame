using System.Windows;

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
                X = Utility.Constants.Constants.BoardHorizontalMargin + Utility.Constants.Constants.CellSize,
                Y = Utility.Constants.Constants.BoardVerticalMargin + Utility.Constants.Constants.CellSize
            };

            if (point == AllPossiblePoints[0])
                return AllPossiblePoints;

            AllPossiblePoints.Clear();
        }
        
        for (int i = 0; i < Utility.Constants.Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Utility.Constants.Constants.CountOfCells; j++)
            {
                var point = new Point
                {
                    X = Utility.Constants.Constants.BoardHorizontalMargin + j * Utility.Constants.Constants.CellSize,
                    Y = Utility.Constants.Constants.BoardVerticalMargin + i * Utility.Constants.Constants.CellSize
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

            if (currMinLength <= Utility.Constants.Constants.StoneSize / 2.0)
            {
                return pt;
            }
        }

        return Utility.Constants.Constants.UndefinedPoint;
    }
}