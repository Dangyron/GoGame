using System.Collections;

namespace GoGame.Utility.Models;

public class StonesGroup : IEnumerable<StoneIndexer>, ICloneable
{
    public StonesStates Colour { get; }
    
    private readonly HashSet<StoneIndexer> _group;

    public StonesGroup(StonesStates colour)
    {
        Colour = colour;
        _group = new HashSet<StoneIndexer>();
    }

    public bool Contains(StoneIndexer indexer) => _group.Contains(indexer);

    public int Count => _group.Count;

    public bool Add(StoneIndexer indexer) => _group.Add(indexer);

    public StoneIndexer this[int index] => _group.ElementAt(index);
    public void Clear() => _group.Clear();

    public IEnumerator<StoneIndexer> GetEnumerator()
    {
        return _group.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public object Clone()
    {
        var group = new StonesGroup(Colour);

        foreach (var stone in _group)
        {
            group.Add(stone);
        }

        return group;
    }
}