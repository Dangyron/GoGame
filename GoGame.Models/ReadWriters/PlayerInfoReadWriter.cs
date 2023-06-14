using System.IO;
using GoGame.Models.Models;
using GoGame.Utility;

namespace GoGame.Models.ReadWriters;

public class PlayerInfoReadWriter : IReadWriter<PlayerDataEventArgs>
{
    public PlayerDataEventArgs ReadFromFile()
    {
        var data = File.ReadAllText("Players.txt").Split(';').ToArray();
        return new PlayerDataEventArgs(data[0], data[1], data[2]);
    }

    public void WriteToFile(PlayerDataEventArgs type)
    {
        File.WriteAllText("Players.txt", $"{type.Player1Name};{type.Player2Name};{type.ChosenColor}");
    }

    public void Clear()
    {
        File.WriteAllText("Players.txt", "");
    }
}