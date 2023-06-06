using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Models;

public class StonesList
{
    private readonly List<List<Stone>> _stones;

    public StonesList()
    {
        _stones = new List<List<Stone>>(Constants.CountOfCells);
        InitStones();
    }

    public Stone this[StoneIndexer index] => _stones[index.I][index.J];

    public List<Stone> this[int i] => _stones[i];

    public int Count => _stones.Count;

    public void Place(StoneIndexer index, Stone stone)
    {
        _stones[index.I][index.J] = stone;
    }
    
    public void Place(int i, int j, Stone stone)
    {
        _stones[i][j] = stone;
    }
    
    public void Remove(StoneIndexer position)
    {
        _stones[position.I][position.J] = new Stone(StonesStates.Empty);
    }
    
    private void InitStones()
    {
        for (var i = 0; i < Constants.CountOfCells; i++)
        {
            _stones.Add(new List<Stone>(Constants.CountOfCells));
            
            for (var j = 0; j < Constants.CountOfCells; j++)
            {
                _stones[i].Add(new Stone(StonesStates.Empty));
            }
        }
    }
    
    public static bool operator ==(StonesList a, StonesList b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a._stones.SequenceEqual(b._stones);
    }

    public static bool operator !=(StonesList a, StonesList b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, null) || GetType() != obj.GetType())
            return false;

        return this == (StonesList)obj;
    }

    public override int GetHashCode()
    {
        return _stones.GetHashCode();
    }
}