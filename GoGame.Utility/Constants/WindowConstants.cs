using System.Windows.Media;

namespace GoGame.Utility.Constants;

public static partial class Constants
{
    public static double WindowWidth { get; set; } = 1920;
    public static double WindowHeight { get; set; } = 1080;
    public static Brush BoardBackGroundColour { get; } = new SolidColorBrush(Color.FromRgb(220,179,92));
}