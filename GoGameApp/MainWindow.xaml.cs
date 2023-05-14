using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGameApp;

public partial class MainWindow
{
    private readonly Board _board;
    private readonly ToolBox _toolBox;
    private readonly IPlayer _firstPlayer;
    private readonly IPlayer _secondPlayer;
    private StonesStates _stonesStates = StonesStates.White;
    private Stone _stone;
    private Ellipse _ellipse;
    private Point _position;
    private readonly Label _label;
    public MainWindow()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _board = new Board(BoardCanvas);
        _toolBox = new ToolBox(BoardCanvas, this);
        
        _stone = new Stone(_stonesStates);
        _label = new Label();
        Canvas.SetLeft(_label, 0);
        Canvas.SetTop(_label, 0);
        BoardCanvas.Children.Add(_label);
        _ellipse = new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = _stonesStates.ConvertStonesColourToBrush()
        };
        //_firstPlayer = new Player(StonesStates.Black, "As", BoardCanvas);
        /*
        
        _secondPlayer = new Player(StonesColour.Black, "As", BoardCanvas);*/
    }

    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        if (sender is not MainWindow currentWindow)
            return;

        Constants.WindowWidth = currentWindow.ActualWidth;
        Constants.WindowHeight = currentWindow.ActualHeight;
        
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        BoardCanvas.Children.Clear();
        _board.Draw();
        _toolBox.SetToolBox();
        BoardCanvas.Children.Add(_label);
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        //BoardCanvas.Children.Add(_stone.GetStone());
        /*GameController.StartGame(_firstPlayer, _secondPlayer, default);*/
    }

    private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        _position = e.GetPosition(this);

        _label.Content = $"X = {_position.X}, Y = {_position.Y} {ActualWidth} {ActualHeight}";
        
        if (_position.X < Constants.BoardHorizontalMargin - Constants.StoneSize
            || _position.X > Constants.BoardHorizontalMargin + Constants.GridLineLength + Constants.StoneSize)
            return;
        
        if (_position.Y < Constants.BoardVerticalMargin - Constants.StoneSize
            || _position.Y > Constants.BoardVerticalMargin + Constants.GridLineLength + Constants.StoneSize)
            return;
        
        /*
        Canvas.SetLeft(_stone.GetStone(), _position.X);
        Canvas.SetTop(_stone.GetStone(), _position.Y);
        */
        
        
    }

    private void BoardCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(BoardCanvas);
        if (e.OriginalSource is Image { Name: "Maximize" })
        {
            WindowState = WindowState switch
            {
                WindowState.Maximized => WindowState.Normal,
                WindowState.Normal => WindowState.Maximized,
                _ => WindowState
            };
            
            return;
        }
        if (mousePosition.Y is > 5 and <= Constants.ToolBoxElementsSize)
            if (mousePosition.X < Constants.WindowWidth - Constants.ToolBoxElementsSize * 2 - Constants.ToolBoxActiveElementsSize) 
                DragMove();
        
        
        if (mousePosition.Y <= Constants.ToolBoxActiveElementsSize) return;
        
        if (!(mousePosition.X > Constants.BoardHorizontalMargin - Constants.StoneSize)
            || !(mousePosition.X < Constants.BoardHorizontalMargin + Constants.StoneSize + Constants.GridLineLength)) return;

        if (!_board.AddStone(_stone, mousePosition)) return;
        
        _stonesStates = _stonesStates.GetNextColour();
        _stone = new Stone(_stonesStates);
        UpdateBoard();


    }
}