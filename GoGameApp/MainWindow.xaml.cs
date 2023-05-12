using System.Windows;
using System.Windows.Input;
using GoGame.Utility;

namespace GoGameApp;

public partial class MainWindow
{
    private readonly Board _board;
    private readonly ToolBox _toolBox;
    public MainWindow()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _board = new Board(BoardCanvas);
        _toolBox = new ToolBox(BoardCanvas, this);
        Width = Constants.WindowWidth;
        Height = Constants.WindowHeight;
        _board.Draw();
        _toolBox.SetToolBox();
    }

    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        if (sender is not MainWindow currentWindow)
            return;

        Constants.WindowWidth = currentWindow.ActualWidth;
        Constants.WindowHeight = currentWindow.ActualHeight;
        
        BoardCanvas.Children.Clear();
        _board.Draw();
        _toolBox.SetToolBox();
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(this);
        
        if (mousePosition.Y is > 5 and <= 10)
            DragMove();
    }
}