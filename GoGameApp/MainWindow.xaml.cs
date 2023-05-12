using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp;

public partial class MainWindow
{
    private readonly Board _board;
    public MainWindow()
    {
        InitializeComponent();
        _board = new Board(BoardCanvas);
        Width = Constants.WindowWidth;
        Height = Constants.WindowHeight;
        _board.Draw();
    }
}