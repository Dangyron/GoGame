using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Helpers;

public static class StonesHelper
{
    public static StonesStates GetOppositeStoneState(this StonesStates stonesState)
    {
        return stonesState switch
        {
            StonesStates.White => StonesStates.Black,
            StonesStates.Black => StonesStates.White,
            _ => throw new InvalidEnumArgumentException(nameof(stonesState))
        };
    }
    
    public static Brush ConvertStonesColourToBrush(this StonesStates stonesState)
    {
        return stonesState switch
        {
            StonesStates.White => Constants.WhiteStone,
            StonesStates.Black => Constants.BlackStone,
            StonesStates.Empty => Constants.EmptyStone,
            _ => throw new ArgumentOutOfRangeException(nameof(stonesState), stonesState, null)
        };
    }
    public static Brush ConvertStonesColourToMouseBrush(this StonesStates stonesState, bool isOnFreePoint = false)
    {
        if (isOnFreePoint)
            return stonesState switch
            {
                StonesStates.White => Constants.MouseWhiteStone,
                StonesStates.Black => Constants.MouseBlackStone,
                _ => throw new InvalidEnumArgumentException(nameof(stonesState))
            };
        
        return Constants.EmptyStone;
    }
    
    public static Ellipse ConvertStonesColourToEllipse(this StonesStates stonesState)
    {
        return new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = stonesState.ConvertStonesColourToBrush()
        };
    }
    
    public static StoneIndexer ConvertPositionToIndexers(this Point point)
    {
        var position = point.GetNearestPositionOnBoard();
        
        if (position == Constants.UndefinedPoint)
            return Constants.UndefinedIndexer;

        int i = (int)(position.Y - Constants.BoardVerticalMargin) / Constants.StoneSize;
        int j = (int)(position.X - Constants.BoardHorizontalMargin) / Constants.StoneSize;

        return new StoneIndexer { I = i, J = j };
    }
}