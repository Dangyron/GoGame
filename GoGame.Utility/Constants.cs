using System.Windows.Media;

namespace GoGame.Utility;

public static class Constants
{
    public static double WindowWidth { get; set; } = 1920;
    public static double WindowHeight { get; set; } = 1080;
    public static Brush BoardBackGroundColour { get; } = new SolidColorBrush(Color.FromRgb(220,179,92));
    public const int CountOfCells = 19;
    public static double BoardVerticalMargin => (WindowHeight - GridLineLength) / 2 + ToolBoxTopPosition;
    public static double BoardHorizontalMargin => (WindowWidth - GridLineLength) / 2;
    public static double CellSize => Math.Min(WindowHeight, WindowWidth) / CountOfCells;
    public static double GridLineLength => CellSize * (CountOfCells - 1);

    public static int BoardStrokeThickness => 2;
    public static Brush BoardStrokeColour { get; } = Brushes.Black;
    public const int BoardStarSize = 10;

    public static Brush WhiteStone { get; } = Brushes.White;
    public static Brush BlackStone { get; } = Brushes.Black;

    public const int StoneSize = 20;

    public const int ToolBoxElementsSize = 20;
    public const int ToolBoxActiveElementsSize = 22;

    public const int CrossImageRightPosition = 10;
    public const int MinimizeImageRightPosition = 40;
    public const int ToolBoxTopPosition = 10;
}