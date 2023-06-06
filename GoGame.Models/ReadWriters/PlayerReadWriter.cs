using System.IO;
using GoGame.Utility;

namespace GoGame.Models.ReadWriters;

public class PlayerReadWriter : IReadWriter<StonesStates>
{
    public void WriteToFile(StonesStates colour)
    {
        File.WriteAllText("LastPlayer.txt", colour == StonesStates.White ? "1" : "-1");
    }
    
    public StonesStates ReadFromFile()
    {
        return File.ReadAllText("LastPlayer.txt") == "1" ? StonesStates.White : StonesStates.Black;
    }
    
    public void Clear()
    {
        File.WriteAllText("LastPlayer.txt", "1");
    }
}