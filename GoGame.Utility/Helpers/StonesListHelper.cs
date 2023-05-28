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
            dummyGroups.Add(new StonesGroup(stones[position].StoneStates));
            dummyGroups.Last().Add(position);

            if (dummyGroups.Last().HasDome(stones, position))
            {
                groups.Add(dummyGroups.Last());
                return true;
            }
            dummyGroups.Clear();
            bool hasDome = true;
            
            foreach (var group in groups)
            {
                var adjacentPositions = position.GetAdjacentPositions();
                foreach (var adjacentPosition in adjacentPositions)
                {
                    if (stones[position].StoneStates.GetOppositeStoneState() == group.Colour && group.Contains(adjacentPosition))
                    {
                        if (!group.HasDome(stones, adjacentPosition))
                        {
                            hasDome = false;
                        }
                    }
                }
            }

            if (!hasDome) 
                return true;
            
            stones.Remove(position);
            Notification.SelfCaptured();
            return false;
        }
        
        var merged = dummyGroups.MergeGroups(position);

        if (merged.HasDome(stones, position))
        {
            groups[indexesOfGroups.First()] = merged;
            for (int i = 1; i < indexesOfGroups.Count; i++)
                groups[indexesOfGroups[i]].Clear();

            return true;
        }
        stones.Remove(position);
        Notification.SelfCaptured();

        return false;
    }
}