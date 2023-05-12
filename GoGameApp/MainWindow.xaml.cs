using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp;

public partial class MainWindow
{
    private readonly Board _board;
    private readonly ToolBox _toolBox;
    private readonly IPlayer _firstPlayer;
    private readonly IPlayer _secondPlayer;
    private readonly Stone _stone;
    public MainWindow()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _board = new Board(BoardCanvas);
        _toolBox = new ToolBox(BoardCanvas, this);
        Width = Constants.WindowWidth;
        Height = Constants.WindowHeight;
        _stone = new Stone(StonesColour.Black);
        Canvas.SetLeft(_stone.GetStone(), 0);
        Canvas.SetTop(_stone.GetStone(), 0);
        /*
        _firstPlayer = new Player(StonesColour.Black, "As", BoardCanvas);
        _secondPlayer = new Player(StonesColour.Black, "As", BoardCanvas);*/
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
        BoardCanvas.Children.Add(_stone.GetStone());
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(this);
        
        if (mousePosition.Y is > 5 and <= 15)
            DragMove();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        _board.Draw();
        _toolBox.SetToolBox();
        //BoardCanvas.Children.Add(_stone.GetStone());
        /*GameController.StartGame(_firstPlayer, _secondPlayer, default);*/
    }

    private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        var point = e.GetPosition(BoardCanvas);
        Canvas.SetLeft(_stone.GetStone(), point.X - Constants.StoneSize / 2.0);
        Canvas.SetTop(_stone.GetStone(), point.Y - Constants.StoneSize / 2.0);
    }
}