using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GoGame.Utility.Helpers;

public static class StonesHelper
{
    private static readonly List<Point> AllPossiblePoints = new();

    public static StonesStates GetNextColour(this StonesStates states)
    {
        return states switch
        {
            StonesStates.Black => StonesStates.White,
            StonesStates.White => StonesStates.Black,
            StonesStates.Empty => throw new InvalidEnumArgumentException(nameof(states)),
            _ => throw new ArgumentOutOfRangeException(nameof(states), states, null)
        };
    }

    public static Brush ConvertStonesColourToBrush(this StonesStates stonesStates)
    {
        return stonesStates switch
        {
            StonesStates.White => Constants.Constants.WhiteStone,
            StonesStates.Black => Constants.Constants.BlackStone,
            StonesStates.Empty => Constants.Constants.EmptyStone,
            _ => throw new ArgumentOutOfRangeException(nameof(stonesStates), stonesStates, null)
        };
    }
    public static Brush ConvertStonesColourToMouseBrush(this StonesStates stonesStates)
    {
        return stonesStates switch
        {
            StonesStates.White => Constants.Constants.MouseWhiteStone,
            StonesStates.Black => Constants.Constants.MouseBlackStone,
            StonesStates.Empty => throw new InvalidEnumArgumentException(nameof(stonesStates)),
            _ => throw new ArgumentOutOfRangeException(nameof(stonesStates), stonesStates, null)
        };
    }

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

    public static Point GetNearestPosition(this Point point)
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
    public static (int I, int J) ConvertPositionToIndexers(this Point point)
    {
        var points = GetAllPossiblePoints();
        var i = 0;
        int j = -1;

        foreach (var pt in points)
        {
            j++;
            if (j == Constants.Constants.CountOfCells)
            {
                j = 0;
                i++;
            }

            double currMinLength = Math.Sqrt(
                (pt.X - point.X) *
                (pt.X - point.X) +
                (pt.Y - point.Y) *
                (pt.Y - point.Y));

            if (currMinLength <= Constants.Constants.StoneSize / 2.0)
            {
                return (i, j);
            }
        }

        return Constants.Constants.UndefinedIndexer;
    }

    public static void UpdateSize(this Ellipse ellipse)
    {
        ellipse.Width = Constants.Constants.StoneSize;
        ellipse.Height = Constants.Constants.StoneSize;
    }
}