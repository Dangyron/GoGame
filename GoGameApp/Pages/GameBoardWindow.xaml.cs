using GoGame.Models.Models;
using GoGameApp.Windows;

namespace GoGameApp.Pages;

public partial class GameBoardWindow
{
    private readonly GameController _gameController;
    public GameBoardWindow(PlayerDataEventArgs startGame)
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        
        _gameController = new GameController(BoardCanvas, this, startGame);
        MouseMove += _gameController.MouseMove;
    }

    public void ChangeBoardSize(Size size)
    {
        Width = size.Width;
        Height = size.Height - 40;
        
        _gameController.UpdateBoard();
    }
    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        Constants.WindowWidth = BoardCanvas.ActualWidth;
        Constants.WindowHeight = BoardCanvas.ActualHeight;

        _gameController.UpdateBoard();
    }

    private void GameBoard_OnLoaded(object sender, RoutedEventArgs e)
    {
        _ = _gameController.StartGame(default);
    }
}