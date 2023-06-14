using System.Diagnostics;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using GoGame.Models.Helpers;
using GoGame.Models.ReadWriters;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Models;

public class Player : IPlayer
{
    private TaskCompletionSource<bool>? _moveCompletionSource;
    private readonly Board _board;
    public StonesStates StoneColour { get; }
    public string Name { get; }

    public bool Resign { get; private set; }
    public int SkippedCount { get; private set; }
    public int CapturedStones { get; private set; }
    public bool HasMouse => true;
    private Stone _stone;
    public Ellipse Mouse { get; }
    private readonly DispatcherTimer _timer;

    private readonly CancellationTokenSource _cancellationToken;
    public Player(StonesStates stoneColour, string name, Board board, int capturedStones, CancellationTokenSource cancellationToken)
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _timer.Tick += TimerTick;
        _timer.Start();
        StoneColour = stoneColour;
        Name = name;
        _board = board;
        _stone = new Stone(StoneColour);
        CapturedStones = capturedStones;
        _cancellationToken = cancellationToken;
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

        StartMove();
        
        await _moveCompletionSource.Task;
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        if (Resign)
        {
            EndMove();
            _moveCompletionSource?.TrySetCanceled();
        }
    }

    public void OnResign(object sender, RoutedEventArgs e)
    {
        Resign = true;
        _cancellationToken.Cancel();
    }

    public void StopMoving()
    {
        if (!Resign)
        {
            EndMove();
            _moveCompletionSource?.TrySetCanceled();
        }
    }

    private void StartMove()
    {
        int count = _board.StoneCaptured?.GetInvocationList().Length ?? 0;
        if (count == 0)
        {
            _board.StoneCaptured += AddCapturedStones;
            _board.BoardCanvas.MouseMove += MouseMoveHandler;
            _board.BoardCanvas.MouseLeftButtonDown += LeftButtonClickHandler;
            _board.BoardCanvas.MouseRightButtonDown += RightButtonClickHandler;
        }
    }

    private void AddCapturedStones(int count)
    {
        var capturedStones = new CapturedStonesReadWriter().ReadFromFile();
        int index = StoneColour == StonesStates.Black ? 0 : 1;
        CapturedStones += count;
        capturedStones[index] += count;
        new CapturedStonesReadWriter().WriteToFile(capturedStones);
    }

    private void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_board.BoardCanvas);
            
        var nearest = position.GetNearestPositionOnBoard();
        
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
        if (!mousePosition.IsMouseOnBoard()) return;

        if (!_board.AddStone(_stone, mousePosition)) return;
        _stone = new Stone(StoneColour);

        EndMove();
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
        SkippedCount = 0;
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
        SkippedCount++;
        _moveCompletionSource!.SetResult(true);
        DeactivateMouse();
    }

    private void UpdateEllipseColour(Point position)
    {
        Mouse.Fill = StoneColour.ConvertStonesColourToMouseBrush(!_board.ContainsStoneAtThisPosition(position));
    }

    private void DeactivateMouse()
    {
        Mouse.Fill = Constants.EmptyStone;
    }
}