using System.IO;
using System.Text;
using GoGame.Models.Models;

namespace GoGame.Models.ReadWriters;

public class SettingsReadWriter : IReadWriter<SettingDto>
{
    public SettingDto ReadFromFile()
    {
        var file = File.ReadAllText("Settings.txt").Split(';').ToArray();
        return new SettingDto
        {
            BoardSize = int.Parse(file[0]),
            BoardColour = file[1],
            LineColour = file[2],
            StoneType = file[3],
        };
    }

    public void WriteToFile(SettingDto type)
    {
        using var writer = new StreamWriter("Settings.txt");
        writer.Write($"{type.BoardSize};");
        writer.Write($"{ConvertToValidColourFormat(type.BoardColour)};");
        writer.Write($"{ConvertToValidColourFormat(type.LineColour)};");
        writer.Write($"{type.StoneType}");
    }

    public void Clear()
    {
        WriteToFile(new SettingDto());
    }

    public static int GetCountOfCellsFromFile() => int.Parse(File.ReadAllText("Settings.txt").Split(';').First());
    
    public static string GetBackColourOfBoard() => File.ReadAllText("Settings.txt").Split(';').Skip(1).First();
    
    public static string GetLineColourOfBoard() => File.ReadAllText("Settings.txt").Split(';').Skip(2).First();
    
    public static string GetTypeOfStone() => File.ReadAllText("Settings.txt").Split(';').Last();

    private string ConvertToValidColourFormat(string colour)
    {
        return colour.Replace('_', '0');
    }
}