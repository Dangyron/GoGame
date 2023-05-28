using GoGame.Models.Models;
using GoGame.Utility;

namespace GoGame.Models.Helpers;

public static class RuleHelper
{
    public static bool CheckPlacingRules(this StonesList current, Stone stone, StoneIndexer position)
    {
        if (!position.IsValidIndex())
        {
            return false;
        }

        if (!current.CheckIfClickedPointFree(position))
        {
            Notification.InvalidMove();
            return false;
        }

        current.Place(position, stone);
        
        return true;
    }

    private static bool CheckIfClickedPointFree(this StonesList current, StoneIndexer position)
        => current[position].StoneStates == StonesStates.Empty;

    public static IEnumerable<StonesGroup> CaptureStones(this List<StonesGroup> groups, StonesList stones, StonesStates colour, StoneIndexer position)
    {
        foreach (var group in groups)
        {
            if (!group.HasDome(stones, position))
            {
                yield return group;
            }
        }
    }
}