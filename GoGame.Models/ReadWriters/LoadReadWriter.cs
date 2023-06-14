using System.IO;

namespace GoGame.Models.ReadWriters;

public class LoadReadWriter : IReadWriter<bool>
{
    public bool ReadFromFile()
    {
        return Convert.ToBoolean(File.ReadAllText("Load.txt"));
    }

    public void WriteToFile(bool type)
    {
        File.WriteAllText("Load.txt", type.ToString());
    }

    public void Clear()
    {
        File.WriteAllText("Load.txt", false.ToString());
    }
}