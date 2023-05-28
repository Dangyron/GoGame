using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Models.Helpers;
using GoGame.Utility;

namespace GoGame.Models.Models;

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
            Width = Utility.Constants.Constants.StoneSize,
            Height = Utility.Constants.Constants.StoneSize,
            Fill = StoneColour.ConvertStonesColourToMouseBrush()
        };
        DeactivateMouse();
    }

    public async Task Move()
    {
        _moveCompletionSource = new TaskCompletionSource<bool>();

        StartMove();

        await _moveCompletionSource.Task;
    }

    private void StartMove()
    {
        _board.StoneCaptured += AddCapturedStones;
        _board.BoardCanvas.MouseMove += MouseMoveHandler;
        _board.BoardCanvas.MouseLeftButtonDown += LeftButtonClickHandler;
        _board.BoardCanvas.MouseRightButtonDown += RightButtonClickHandler;
    }

    private void AddCapturedStones(int count) => CapturedStones += count;
    
    private void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_board.BoardCanvas);
        var nearest = position.GetNearestPositionOnBoard();
        
        if (!position.IsMouseOnBoard() || nearest == Utility.Constants.Constants.UndefinedPoint)
        {
            DeactivateMouse();
            return;
        }
        
        UpdateEllipseColour(nearest);

        Canvas.SetLeft(Mouse, nearest.X - Utility.Constants.Constants.StoneSize / 2.0);
        Canvas.SetTop(Mouse, nearest.Y - Utility.Constants.Constants.StoneSize / 2.0);
    }

    private void LeftButtonClickHandler(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(_board.BoardCanvas);
        if (!mousePosition.IsMouseOnBoard()) return;

        if (!_board.AddStone(_stone, mousePosition)) return;
        Debug.WriteLine($"Current stones count: {CapturedStones}");
        _stone = new Stone(StoneColour);

        EndMove();
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void EndMove()
    {
        _board.BoardCanvas.MouseLeftButtonDown -= LeftButtonClickHandler;
        _board.BoardCanvas.MouseRightButtonDown -= RightButtonClickHandler;
        _board.BoardCanvas.MouseMove -= MouseMoveHandler;
        _board.StoneCaptured -= AddCapturedStones;
    }

    private void RightButtonClickHandler(object sender, MouseButtonEventArgs e)
    {
        EndMove();
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void UpdateEllipseColour(Point position)
    {
        Mouse.Fill = StoneColour.ConvertStonesColourToMouseBrush(!_board.ContainsStoneAtThisPosition(position));
    }

    private void DeactivateMouse()
    {
        Mouse.Fill = Utility.Constants.Constants.EmptyStone;
    }
}