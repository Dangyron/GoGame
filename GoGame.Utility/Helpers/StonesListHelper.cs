using GoGame.Utility.Models;

namespace GoGame.Utility.Helpers;

public static class StonesListHelper
{
    public static void RemoveGroup(this StonesList stones, StonesGroup group)
    {
        foreach (var element in group)
            stones.Remove(element);

        group.Clear();
    }

    public static bool TryAddToGroup(this StonesList stones, List<StonesGroup> groups, StoneIndexer position)
    {
        var indexesOfGroups = new List<int>();
        var dummyGroups = new List<StonesGroup>();
        
        bool added = false;
        for (var i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var adjacentPositions = position.GetAdjacentPositions();
            foreach (var adjacentPosition in adjacentPositions)
            {
                if (stones[position].StoneStates == group.Colour && group.Contains(adjacentPosition))
                {
                    dummyGroups.Add((StonesGroup)group.Clone());
                    dummyGroups.Last().Add(position);
                    added = true;
                    indexesOfGroups.Add(i);
                }
            }
        }

        if (!added)
        {
            groups.Add(new StonesGroup(stones[position].StoneStates));
            groups.Last().Add(position);
            return true;
        }

        var merged = dummyGroups.MergeGroups(position);

        if (merged.HasDome(stones, position))
        {
            groups[0] = merged;
            for (int i = 1; i < indexesOfGroups.Count; i++)
                groups[i].Clear();

            return true;
        }
        stones.Remove(position);
        Notification.SelfCaptured();

        return false;
    }
}