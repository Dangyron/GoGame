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
    public readonly GameBoard GameBoard;
    private readonly Image _returnImage;
    private readonly Board _board;
    private readonly BoardInterface _boardInterface;
    private readonly IPlayer[] _players;
    private bool _isGameOver = false;

    public GameController(Canvas boardCanvas, GameBoard gameBoard)
    {
        _boardCanvas = boardCanvas;
        GameBoard = gameBoard;
        _board = new Board(boardCanvas);
        var captured = new CapturedStonesReadWriter().ReadFromFile();
        _players = new IPlayer[]
        {
            new Player(StonesStates.Black, "Pidoras1", _board, captured[0]),
            new Player(StonesStates.White, "Loh", _board, captured[1])
        };
        _boardInterface = new BoardInterface(boardCanvas, _players);
        _returnImage = new Image
        {
            Height = 15,
            Width = 15,
            Source = new BitmapImage(new Uri(@"D:\riderRepos\GoGame\GoGame.Utility\Images\arrow.png"))
        };
        
        _returnImage.MouseEnter += ReturnImageOnMouseEnter;
        _returnImage.MouseLeave += ReturnImageOnMouseLeave;
        _returnImage.MouseLeftButtonDown += ReturnImageOnMouseLeftButtonDown;
        
        Canvas.SetTop(_returnImage, 10);
        Canvas.SetLeft(_returnImage, 10);
    }
    
    public async Task StartGame(CancellationToken token)
    {
        if (new PlayerReadWriter().ReadFromFile() == StonesStates.Black)
            Array.Reverse(_players);
        
        while (!_isGameOver)
        {
            await _players[0].Move();
            new PlayerReadWriter().WriteToFile(_players[0].StoneColour);
            UpdateBoard();
            
            await _players[1].Move();
            new PlayerReadWriter().WriteToFile(_players[1].StoneColour);
            UpdateBoard();
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