using GoGame.Utility;

namespace GoGame.Models.Helpers;

public static class StoneIndexerHelper
{
    public static List<StoneIndexer> GetAdjacentPositions(this StoneIndexer indexer)
        => new ()
        {
            indexer with { J = indexer.J - 1 },
            indexer with { I = indexer.I - 1 },
            indexer with { J = indexer.J + 1 },
            indexer with { I = indexer.I + 1 },
        };
    
    public static bool IsValidIndex(this StoneIndexer indexer)
    {
        return indexer.I is >= 0 and < Utility.Constants.Constants.CountOfCells &&
               indexer.J is >= 0 and < Utility.Constants.Constants.CountOfCells;
    }
}