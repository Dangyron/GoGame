using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
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
            StonesStates.White => Utility.Constants.Constants.WhiteStone,
            StonesStates.Black => Utility.Constants.Constants.BlackStone,
            StonesStates.Empty => Utility.Constants.Constants.EmptyStone,
            _ => throw new ArgumentOutOfRangeException(nameof(stonesState), stonesState, null)
        };
    }
    public static Brush ConvertStonesColourToMouseBrush(this StonesStates stonesState, bool isOnFreePoint = false)
    {
        if (isOnFreePoint)
            return stonesState switch
            {
                StonesStates.White => Utility.Constants.Constants.MouseWhiteStone,
                StonesStates.Black => Utility.Constants.Constants.MouseBlackStone,
                _ => throw new InvalidEnumArgumentException(nameof(stonesState))
            };
        
        return Utility.Constants.Constants.EmptyStone;
    }
    
    public static StoneIndexer ConvertPositionToIndexers(this Point point)
    {
        var position = point.GetNearestPositionOnBoard();
        
        if (position == Utility.Constants.Constants.UndefinedPoint)
            return Constants.UndefinedIndexer;

        int i = (int)(position.Y - Utility.Constants.Constants.BoardVerticalMargin) / Utility.Constants.Constants.StoneSize;
        int j = (int)(position.X - Utility.Constants.Constants.BoardHorizontalMargin) / Utility.Constants.Constants.StoneSize;

        return new StoneIndexer { I = i, J = j };
    }
}