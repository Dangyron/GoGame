using System.Windows;
using System.Windows.Media;

namespace GoGame.Utility;

public static class StonesHelper
{
    public static StonesStates GetNextColour(this StonesStates states)
    {
        return states switch
        {
            StonesStates.Black => StonesStates.White,
            StonesStates.White => StonesStates.Black,
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

    public static List<Point> GetAllPossiblePoints()
    {
        List<Point> points = new();
        for (int i = 0; i < Constants.Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.Constants.CountOfCells; j++)
            {
                var point = new Point
                {
                    X = Constants.Constants.BoardHorizontalMargin + j * Constants.Constants.CellSize,
                    Y = Constants.Constants.BoardVerticalMargin + i * Constants.Constants.CellSize
                };

                points.Add(point);
            }
        }

        return points;
    }

    public static StoneIndexer ConvertPositionToIndexers(this Point point)
    {
        var points = GetAllPossiblePoints();
        int i = 0;
        int j = -1;
        double minLength = Math.Sqrt(
            (points[0].X - point.X) *
            (points[0].X - point.X) +
            (points[0].Y - point.Y) *
            (points[0].Y - point.Y));

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

            if (currMinLength > minLength) continue;
            if (currMinLength <= Constants.Constants.StoneSize / 2.0)
            {
                return new StoneIndexer { I = i, J = j };
            }

            minLength = currMinLength;
        }

        throw new ArgumentOutOfRangeException(nameof(point));
    }
}