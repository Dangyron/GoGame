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
    private static PlayerDataEventArgs? _startGame;
    private readonly Image _returnImage;
    private readonly Board _board;
    private readonly BoardInterface _boardInterface;
    private IPlayer[] _players;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public GameController(Canvas boardCanvas, GameBoardWindow gameBoard, PlayerDataEventArgs startGame)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _boardCanvas = boardCanvas;
        GameBoard = gameBoard;
        _startGame = startGame;
        _board = new Board(boardCanvas);
        
        InitPlayers(startGame);
        
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
        var res = MessageBox.Show(Application.Current.MainWindow, "Are you sure, that you want to resign", "Resign",
            MessageBoxButton.YesNo);

        if (res == MessageBoxResult.No)
        {
            return;
        }
        
        var button = sender as Button;
        var winner = _players.GetEnemyColour(button!.Name == "Black" ? StonesStates.Black : StonesStates.White);
        _players.Single(i => i.StoneColour != winner).OnResign(new object(), new RoutedEventArgs());
        _players.Single(i => i.StoneColour == winner).StopMoving();
        
        Notification.CongratulationWinner(_players.Select(i => i.Name).ToArray(), _board.Score);
    }

    private void InitPlayers(PlayerDataEventArgs startGame)
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

    public async Task StartGame()
    {
        if (new PlayerReadWriter().ReadFromFile() == StonesStates.Black)
            Array.Reverse(_players);
        
        while (true)
        {
            try
            {
                if (await PlayerMove(0))
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                if (await PlayerMove(1))
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                ReadWriteHelper.ClearAllData();
                if (Application.Current.MainWindow is not StartUpWindow mainWindow)
                    return;
        
                _cancellationTokenSource.Dispose();
                StartUpWindow.IsGameLoaded = true;
                _startGame = null;
                
                var page = new StartUpPage(false);
                mainWindow.Content = page;
                mainWindow.BindAllButtons(page);
                mainWindow.Show();
                return;
            }
        }
    }

    private async Task<bool> PlayerMove(int playerIndex)
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
                Notification.ShowScore(_board.Score);
                Notification.CongratulationWinner(_players.Select(i => i.Name).ToArray(), _board.Score);
            }

            _cancellationTokenSource.Cancel();
            return true;
        }

        await _players[playerIndex].Move();
        new PlayerReadWriter().WriteToFile(_players[playerIndex].StoneColour);
        UpdateBoard();
        return false;
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
    
    private void ReturnImageOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (Application.Current.MainWindow is not StartUpWindow mainWindow)
            return;
        
        var page = new StartUpPage(false);
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