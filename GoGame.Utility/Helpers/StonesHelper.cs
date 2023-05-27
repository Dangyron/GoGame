using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace GoGame.Utility.Helpers;

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
            StonesStates.White => Constants.Constants.WhiteStone,
            StonesStates.Black => Constants.Constants.BlackStone,
            StonesStates.Empty => Constants.Constants.EmptyStone,
            _ => throw new ArgumentOutOfRangeException(nameof(stonesState), stonesState, null)
        };
    }
    public static Brush ConvertStonesColourToMouseBrush(this StonesStates stonesState, bool isOnFreePoint = false)
    {
        if (isOnFreePoint)
            return stonesState switch
            {
                StonesStates.White => Constants.Constants.MouseWhiteStone,
                StonesStates.Black => Constants.Constants.MouseBlackStone,
                _ => throw new InvalidEnumArgumentException(nameof(stonesState))
            };
        
        return Constants.Constants.EmptyStone;
    }
    
    public static StoneIndexer ConvertPositionToIndexers(this Point point)
    {
        var position = point.GetNearestPositionOnBoard();
        
        if (position == Constants.Constants.UndefinedPoint)
            return Constants.Constants.UndefinedIndexer;

        int i = (int)(position.Y - Constants.Constants.BoardVerticalMargin) / Constants.Constants.StoneSize;
        int j = (int)(position.X - Constants.Constants.BoardHorizontalMargin) / Constants.Constants.StoneSize;

        return new StoneIndexer { I = i, J = j };
    }
}