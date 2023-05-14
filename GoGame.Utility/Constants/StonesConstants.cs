using System.Windows.Media;

namespace GoGame.Utility.Constants;

public static partial class Constants
{
    public static Brush WhiteStone { get; } = Brushes.White;
    public static Brush BlackStone { get; } = Brushes.Black;
    public static Brush EmptyStone { get; } = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

    public const int StoneSize = 30;
}