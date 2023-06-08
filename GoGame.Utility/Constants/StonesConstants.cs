using System.Windows;
using System.Windows.Media;

namespace GoGame.Utility.Constants;

public static partial class Constants
{
    public const double Komi = 6.5;
    public static Brush WhiteStone { get; } = Brushes.White;
    public static Brush BlackStone { get; } = Brushes.Black;
    public static Brush EmptyStone { get; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
    
    public static Brush MouseWhiteStone { get; } = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
    public static Brush MouseBlackStone { get; } = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));

    public static int StoneSize => (int)CellSize;

    public static Point UndefinedPoint { get; } = new() { X = -1, Y = -1 };
    public static StoneIndexer UndefinedIndexer { get; } = new(){ I = -1, J = -1};
}