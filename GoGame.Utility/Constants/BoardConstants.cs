using System.Windows.Media;

namespace GoGame.Utility.Constants;

public static partial class Constants
{
    public const int CountOfCells = 19;
    public static double BoardVerticalMargin => Math.Floor(WindowHeight - GridLineLength - ToolBoxElementsSize) / 2 + ToolBoxElementsSize;
    public static double BoardHorizontalMargin => Math.Floor(WindowWidth - GridLineLength) / 2;
    public static double CellSize => Math.Min(Math.Floor(Math.Min(WindowHeight, WindowWidth) / CountOfCells), 55);
    public static double GridLineLength => CellSize * (CountOfCells - 1);

    public static int BoardStrokeThickness => 2;
    public static Brush BoardStrokeColour { get; } = Brushes.Black;
    public const int BoardStarSize = 10;
}