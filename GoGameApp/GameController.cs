using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGame.Utility;
using GoGameApp.Pages;
using GoGameApp.Windows;

namespace GoGameApp;

public class GameController
{
    private readonly Canvas _boardCanvas;
    public readonly GameBoardWindow GameBoard;
    private readonly Image _returnImage;
    private readonly Board _board;
    private readonly BoardInterface _boardInterface;
    private IPlayer[] _players;
    private bool _isGameOver = false;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public GameController(Canvas boardCanvas, GameBoardWindow gameBoard, PlayerDataEventArgs startGame)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _boardCanvas = boardCanvas;
        GameBoard = gameBoard;
        _board = new Board(boardCanvas);
        
        InitPlayer(startGame);
        
        _boardInterface = new BoardInterface(boardCanvas, _players!);
        _returnImage = new Image
        {
            Height = 15,
            Width = 15,
            Source = new BitmapImage(new Uri(@"D:\riderRepos\GoGame\GoGame.Utility\Images\arrow.png"))
        };
        
        _returnImage.MouseEnter += ReturnImageOnMouseEnter;
        _returnImage.MouseLeave += ReturnImageOnMouseLeave;
        _returnImage.MouseLeftButtonDown += ReturnImageOnMouseLeftButtonDown;

        _boardInterface.ResignButtons[0].Name = "Black";
        _boardInterface.ResignButtons[1].Name = "White";
        
        _boardInterface.ResignButtons[0].Click += OnResign;
        _boardInterface.ResignButtons[1].Click += OnResign;
        
        Canvas.SetTop(_returnImage, 10);
        Canvas.SetLeft(_returnImage, 10);
    }

    private void OnResign(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var winner = _players.GetEnemyName(button!.Name == "Black" ? StonesStates.Black : StonesStates.White);
        _players.Single(i => i.StoneColour == winner).OnResign(null, null);
        Notification.CongratulationWinner(_players.Single(i => i.StoneColour == winner).Name, winner);
    }

    private void InitPlayer(PlayerDataEventArgs startGame)
    {
        var captured = new CapturedStonesReadWriter().ReadFromFile();
        _players = startGame.ChosenColor switch
        {
            "Random" when new Random().Next(2) == 0 => new IPlayer[]
            {
                new Player(StonesStates.Black, startGame.Player1Name, _board, captured[0],
                    _cancellationTokenSource),
                new Player(StonesStates.White, startGame.Player2Name, _board, captured[1], _cancellationTokenSource)
            },
            "Random" or "White" => new IPlayer[]
            {
                new Player(StonesStates.Black, startGame.Player2Name, _board, captured[0], _cancellationTokenSource),
                new Player(StonesStates.White, startGame.Player1Name, _board, captured[1],
                    _cancellationTokenSource)
            },
            _ => new IPlayer[]
            {
                new Player(StonesStates.Black, startGame.Player1Name, _board, captured[0],
                    _cancellationTokenSource),
                new Player(StonesStates.White, startGame.Player2Name, _board, captured[1], _cancellationTokenSource)
            }
        };
    }

    public async Task StartGame(CancellationToken token)
    {
        if (new PlayerReadWriter().ReadFromFile() == StonesStates.Black)
            Array.Reverse(_players);
        try
        {
            while (!_isGameOver)
            {
                if (_players[0].Resign || _players[1].Resign 
                                       || _players[0].SkippedCount == 3 && _players[1].SkippedCount == 3)
                {
                    if (_players[0].SkippedCount == 3 && _players[1].SkippedCount == 3)
                    {
                        var winner = _board.Score[StonesStates.White] - _players[0].CapturedStones + Constants.Komi
                                     > _board.Score[StonesStates.Black] - _players[1].CapturedStones
                            ? _players[1]
                            : _players[0];
                        Notification.CongratulationWinner(winner.Name, winner.StoneColour);
                    }
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    break;
                }
                await _players[0].Move();
                new PlayerReadWriter().WriteToFile(_players[0].StoneColour);
                UpdateBoard();

                if (_players[0].Resign || _players[1].Resign 
                    || _players[0].SkippedCount == 3 && _players[1].SkippedCount == 3)
                {
                    _cancellationTokenSource.Cancel();
                    if (_players[0].SkippedCount == 3 && _players[1].SkippedCount == 3)
                    {
                        var winner = _board.Score[StonesStates.White] - _players[0].CapturedStones + Constants.Komi
                                     > _board.Score[StonesStates.Black] - _players[1].CapturedStones
                            ? _players[1]
                            : _players[0];
                        Notification.CongratulationWinner(winner.Name, winner.StoneColour);
                    }

                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    break;
                }
                await _players[1].Move();
                new PlayerReadWriter().WriteToFile(_players[1].StoneColour);
                UpdateBoard();
            }
        }
        catch (OperationCanceledException ex)
        {
            ReadWriteHelper.ClearAllData();
            if (Application.Current.MainWindow is not StartUpWindow mainWindow)
                return;
        
            var page = new StartUpPage();
            mainWindow.Content = page;
            mainWindow.BindAllButtons(page);
            mainWindow.Show();
        }
        finally
        {
            _cancellationTokenSource.Dispose();
            StartUpWindow._isFirstLoad = true;
        }
    }
    
    public void UpdateBoard()
    {
        _boardCanvas.Children.Clear();
        _board.Draw();
        _boardInterface.Draw();
        AddMouses();
        _players[0].Mouse.UpdateStoneSize();
        _players[1].Mouse.UpdateStoneSize();
        _boardCanvas.Children.Add(_returnImage);
    }

    private void AddMouses()
    {
        if (_players[0].HasMouse)
            _boardCanvas.Children.Add(_players[0].Mouse);
        
        if (_players[1].HasMouse)
            _boardCanvas.Children.Add(_players[1].Mouse);
    }

    public void MouseMove(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_board.BoardCanvas);
        _board.Label.Content = $"X: {position.X}, Y: {position.Y}";
        
    }
    
    private void ReturnImageOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Application.Current.MainWindow is not StartUpWindow mainWindow)
            return;
        
        var page = new StartUpPage();
        mainWindow.Content = page;
        mainWindow.BindAllButtons(page);
        mainWindow.Show();
    }

    private void ReturnImageOnMouseEnter(object sender, MouseEventArgs e)
    {
        _returnImage.Width = 18;
        _returnImage.Height = 18;
    }
    
    private void ReturnImageOnMouseLeave(object sender, MouseEventArgs e)
    {
        _returnImage.Width = 15;
        _returnImage.Height = 15;
    }
}