using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp.Models;

public class Player : IPlayer
{
    private TaskCompletionSource<bool>? _moveCompletionSource;
    private readonly Board _board;
    public StonesStates StoneColour { get; }
    public string Name { get; }

    public bool Resign { get; private set; } = false;
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
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = StoneColour.ConvertStonesColourToMouseBrush()
        };
        DeactivateMouse();
    }

    public async Task Move()
    {
        _moveCompletionSource = new TaskCompletionSource<bool>();

        _board.BoardCanvas.MouseMove += MouseMoveHandler;
        _board.BoardCanvas.MouseLeftButtonDown += LeftButtonClickHandler;

        await _moveCompletionSource.Task;
    }

    private void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_board.BoardCanvas);
        var nearest = position.GetNearestPosition();
        
        if (!position.IsMouseOnBoard() || nearest == Constants.UndefinedPoint)
        {
            DeactivateMouse();
            return;
        }
        
        UpdateEllipseColour(nearest);

        Canvas.SetLeft(Mouse, nearest.X - Constants.StoneSize / 2.0);
        Canvas.SetTop(Mouse, nearest.Y - Constants.StoneSize / 2.0);
    }

    private void LeftButtonClickHandler(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(_board.BoardCanvas);
        if (!(mousePosition.X > Constants.BoardHorizontalMargin - Constants.StoneSize)
            || !(mousePosition.X <
                 Constants.BoardHorizontalMargin + Constants.StoneSize + Constants.GridLineLength)) return;

        if (!_board.AddStone(_stone, mousePosition)) return;

        _stone = new Stone(StoneColour);

        _board.BoardCanvas.MouseLeftButtonDown -= LeftButtonClickHandler;
        _board.BoardCanvas.MouseMove -= MouseMoveHandler;
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void UpdateEllipseColour(Point position)
    {
        if (!_board.ContainsStoneAtThisPosition(position))
        {
            Mouse.Fill = StoneColour.ConvertStonesColourToMouseBrush();
            return;
        }

        Mouse.Fill = Constants.EmptyStone;
    }

    private void DeactivateMouse()
    {
        Mouse.Fill = Constants.EmptyStone;
    }
}