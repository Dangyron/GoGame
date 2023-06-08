using GoGame.Models.Models;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Helpers;

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

    public static Dictionary<StonesStates, int> ComputingScore(this StonesList board, List<StonesGroup> groups)
    {
        var score = new Dictionary<StonesStates, int>()
        {
            { StonesStates.Black, 0 },
            { StonesStates.White, 0 }
        };

        var visited = new HashSet<StoneIndexer>();

        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                var position = new StoneIndexer(i, j);

                if (visited.Contains(position) || board[position].StoneStates == StonesStates.Empty) 
                    continue;
                visited.Add(position);
                var group = groups.Find(x => x.Contains(position));
                if (group is null)
                {
                    return new Dictionary<StonesStates, int>();
                }
                    
                HashSet<StonesStates> surroundingColors = new HashSet<StonesStates>();

                foreach (var stone in group)
                {
                    foreach (var adjacent in stone.GetAdjacentPositions())
                    {
                        if (adjacent.IsValidIndex())
                        {
                            var colour = board[adjacent].StoneStates;
                            if (colour != StonesStates.Empty)
                            {
                                surroundingColors.Add(colour);
                            }
                        }
                    }
                }
                if (surroundingColors.Count == 1)
                {
                    StonesStates groupColor = surroundingColors.Single();
                    score[groupColor] += group.Count;
                }
            }
        }

        return score;
    }
}