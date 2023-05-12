using System.Windows;
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

    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        if (sender is MainWindow currentWindow)
        {
            Constants.WindowWidth = currentWindow.Width;
            Constants.WindowHeight = currentWindow.Height;
            
            _board.Redraw();
        }
    }
}