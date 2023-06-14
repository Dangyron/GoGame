using System.IO;
using GoGame.Models.Models;
using GoGame.Utility;

namespace GoGame.Models.ReadWriters;

public class GroupsReadWriter : IReadWriter<List<StonesGroup>>
{
    public void WriteToFile(List<StonesGroup> type)
    {
        using var writer = new StreamWriter("Groups.txt");
        foreach (var group in type)
        {
            writer.Write(string.Join(";", group));
            writer.WriteLine(";" + group.Colour);
        }
    }

    public List<StonesGroup> ReadFromFile()
    {
        var list = new List<StonesGroup>();
        using var reader = new StreamReader("Groups.txt");
        while (!reader.EndOfStream)
        {
            var groups = reader.ReadLine()?.Split(';').Reverse();
            if (groups is null || !groups.Any())
                continue;
            
            var colour = (StonesStates)Enum.Parse(typeof(StonesStates), groups.First());
            groups = groups.Skip(1);
            var lstGroup = new StonesGroup(colour);
            foreach (var group in groups)
            {
                var indexer = group.Split(',').Select(i => Convert.ToInt32(i)).ToArray();
                lstGroup.Add(new StoneIndexer(indexer[0], indexer[1]));
            }
            
            list.Add(lstGroup);
        }

        return list;
    }

    public void Clear()
    {
        File.WriteAllText("Groups.txt", "");
    }
}