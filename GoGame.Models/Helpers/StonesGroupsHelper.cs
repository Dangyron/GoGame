using GoGame.Models.Models;
using GoGame.Utility;

namespace GoGame.Models.Helpers;

public static class StonesGroupsHelper
{
    public static StonesGroup MergeGroups(this List<StonesGroup> groups, StoneIndexer position)
    {
        var groupsWhichContainsTheSameStone = groups.Where(i => i.Contains(position)).ToList();
        if (groupsWhichContainsTheSameStone.Count == 1)
            return groupsWhichContainsTheSameStone.First();

        var colour = groupsWhichContainsTheSameStone.First().Colour;
        var mergedGroup = new StonesGroup(colour);

        foreach (var group in groupsWhichContainsTheSameStone)
        {
            mergedGroup = mergedGroup.Union(group).ToStonesGroup(colour);
        }

        return mergedGroup;
    }

    public static StonesGroup ToStonesGroup(this IEnumerable<StoneIndexer> enumerable, StonesStates colour)
    {
        var stoneGroup = new StonesGroup(colour);

        foreach (var eStoneIndexer in enumerable)
        {
            stoneGroup.Add(eStoneIndexer);
        }

        return stoneGroup;
    }

    public static bool HasDome(this StonesGroup group, StonesList stones, StoneIndexer position)
    {
        return group.HasDome(stones, position, new HashSet<StoneIndexer>());
    }
    
    private static bool HasDome(this StonesGroup group, StonesList stones, StoneIndexer position, HashSet<StoneIndexer> visitedPositions)
    {
        if (!position.IsValidIndex())
            return false;

        if (stones[position].StoneStates == StonesStates.Empty)
            return true;
        
        if (!visitedPositions.Add(position))
            return false;
        
        foreach (var stone in group)
        {
            var adjacentPositions = stone.GetAdjacentPositions();
            foreach (var adjacentPosition in adjacentPositions)
            {
                if (group.HasDome(stones, adjacentPosition, visitedPositions))
                    return true;
            }
        }

        return false;
    }
}