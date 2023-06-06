using System.IO;

namespace GoGame.Models.ReadWriters;

public class CapturedStonesReadWriter : IReadWriter<int[]>
{
    public int[] ReadFromFile()
    {
        return File.ReadAllText("CapturedStones.txt").Split(';').Select(i => Convert.ToInt32(i)).ToArray();
    }

    public void WriteToFile(int[] capturedStones)
    {  
        File.WriteAllText("CapturedStones.txt", $"{capturedStones[0]};{capturedStones[1]}");
    }

    public void Clear()
    {
        File.WriteAllText("CapturedStones.txt", "0;0");
    }
}