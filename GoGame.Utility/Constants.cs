using System.Windows.Media;

namespace GoGame.Utility;

public static class Constants
{
    public static double WindowWidth { get; set; } = 1920;
    public static double WindowHeight { get; set; } = 1080;
    public static Brush BoardBackGroundColour { get; } = new SolidColorBrush(Color.FromRgb(220,179,92));
    public const int CountOfCells = 19;
    public static double BoardVerticalMargin => 20;
    public static double BoardHorizontalMargin => (WindowWidth - GridLineLength) / 2;
    public static double CellSize { get; } = WindowHeight / CountOfCells - 1;
    public static double GridLineLength { get; } = CellSize * (CountOfCells - 1);

    public static int BoardStrokeThickness => 2;
    public static Brush BoardStrokeColour { get; } = Brushes.Black;
    public const int BoardStarSize = 10;
}