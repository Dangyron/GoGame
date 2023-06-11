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
        }
        
        var merged = dummyGroups.MergeGroups(position);

        if (merged.HasDome(stones, position))
        {
            if (indexesOfGroups.Count == 0)
            {
                groups.Add(new StonesGroup(stones[position].StoneStates));
                groups[^1] = merged;
                return true;
            }
            groups[indexesOfGroups.First()] = merged;
            
            for (int i = 1; i < indexesOfGroups.Count; i++)
                groups[indexesOfGroups[i]].Clear();

            return true;
        }
        bool hasDome = true;
            
        foreach (var group in groups)
        {
            var adjacentPositions = position.GetAdjacentPositions();
            foreach (var adjacentPosition in adjacentPositions)
            {
                if (stones[position].StoneStates.GetOppositeStoneState() == group.Colour &&
                    group.Contains(adjacentPosition))
                {
                    if (!group.HasDome(stones, adjacentPosition))
                    {
                        hasDome = false;
                        break;
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

    public static Dictionary<StonesStates, int> ComputingScore(this StonesList board)
    {
        return new Dictionary<StonesStates, int>()
        {
            { StonesStates.Black, board.ComputingScore(StonesStates.Black) },
            { StonesStates.White, board.ComputingScore(StonesStates.White) },
        };
    }

    private static int ComputingScore(this StonesList board, StonesStates colour)
    {
        int score = 0;
        var visited = new List<List<bool>>(Enumerable.Repeat(new List<bool>(Enumerable.Repeat(false, Constants.CountOfCells)), Constants.CountOfCells));
        var surrounded = new Queue<StoneIndexer>();

        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                var indexer = new StoneIndexer(i, j);
                if (board[indexer].StoneStates == colour)
                {
                    score++;
                    visited[i][j] = true;
                    surrounded.Enqueue(indexer);
                }
            }
        }

        while (surrounded.Count > 0)
        {
            var adjacents = surrounded.Dequeue().GetAdjacentPositions();
            foreach (var adjacent in adjacents)
            {
                if (!adjacent.IsValidIndex())
                    continue;
                
                if (!visited[adjacent.I][adjacent.J] && board[adjacent].StoneStates == StonesStates.Empty)
                {
                    score++;
                    visited[adjacent.I][adjacent.J] = true;
                    surrounded.Enqueue(adjacent);
                }
            }
        }

        return score;
    }
}