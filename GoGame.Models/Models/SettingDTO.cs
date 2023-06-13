namespace GoGame.Models.Models;

public sealed class SettingDto
{
    public int BoardSize { get; set; } = 19;
    public string BoardColour { get; set; } = "220,179,092";
    public string LineColour { get; set; } = "000,000,000";
    public string StoneType { get; set; } = "circle";
}