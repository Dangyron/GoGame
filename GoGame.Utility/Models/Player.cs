using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Utility.Helpers;

namespace GoGame.Utility.Models;

public class Player : IPlayer
{
    private TaskCompletionSource<bool>? _moveCompletionSource;
    private readonly Board _board;
    public StonesStates StoneColour { get; }
    public string Name { get; }

    public bool Resign { get; private set; } = false;
    public int CapturedStones { get; private set; } = 0;
    public bool HasMouse => true;
    private Stone _stone;
    public Ellipse Mouse { get; }

    public Player(StonesStates stoneColour, string name, Board board)
    {
        StoneColour = stoneColour;
        Name = name;
        _board = board;
        _stone = new Stone(StoneColour);
        Mouse = new Ellipse
        {
            Width = Constants.Constants.StoneSize,
            Height = Constants.Constants.StoneSize,
            Fill = StoneColour.ConvertStonesColourToMouseBrush()
        };
        DeactivateMouse();
    }

    public async Task Move()
    {
        _moveCompletionSource = new TaskCompletionSource<bool>();

        _board.StoneCaptured += AddCapturedStones;
        _board.BoardCanvas.MouseMove += MouseMoveHandler;
        _board.BoardCanvas.MouseLeftButtonDown += LeftButtonClickHandler;
        _board.BoardCanvas.MouseRightButtonDown += RightButtonClickHandler;

        await _moveCompletionSource.Task;
    }

    private void AddCapturedStones(int count) => CapturedStones += count;
    
    private void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_board.BoardCanvas);
        var nearest = position.GetNearestPositionOnBoard();
        
        if (!position.IsMouseOnBoard() || nearest == Constants.Constants.UndefinedPoint)
        {
            DeactivateMouse();
            return;
        }
        
        UpdateEllipseColour(nearest);

        Canvas.SetLeft(Mouse, nearest.X - Constants.Constants.StoneSize / 2.0);
        Canvas.SetTop(Mouse, nearest.Y - Constants.Constants.StoneSize / 2.0);
    }

    private void LeftButtonClickHandler(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(_board.BoardCanvas);
        if (!mousePosition.IsMouseOnBoard()) return;

        if (!_board.AddStone(_stone, mousePosition)) return;

        _stone = new Stone(StoneColour);

        _board.BoardCanvas.MouseLeftButtonDown -= LeftButtonClickHandler;
        _board.BoardCanvas.MouseRightButtonDown -= RightButtonClickHandler;
        _board.BoardCanvas.MouseMove -= MouseMoveHandler;
        _board.StoneCaptured -= AddCapturedStones;
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void RightButtonClickHandler(object sender, MouseButtonEventArgs e)
    {
        _board.BoardCanvas.MouseLeftButtonDown -= LeftButtonClickHandler;
        _board.BoardCanvas.MouseRightButtonDown -= RightButtonClickHandler;
        _board.BoardCanvas.MouseMove -= MouseMoveHandler;
        _board.StoneCaptured -= AddCapturedStones;
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void UpdateEllipseColour(Point position)
    {
        Mouse.Fill = StoneColour.ConvertStonesColourToMouseBrush(!_board.ContainsStoneAtThisPosition(position));
    }

    private void DeactivateMouse()
    {
        Mouse.Fill = Constants.Constants.EmptyStone;
    }
}