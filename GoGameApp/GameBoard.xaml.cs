using System.Windows;
using GoGame.Utility.Constants;

namespace GoGameApp;

public partial class GameBoard
{
    private readonly GameController _gameController;
    public GameBoard()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _gameController = new GameController(BoardCanvas);
    }

    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        Constants.WindowWidth = BoardCanvas.ActualWidth;
        Constants.WindowHeight = BoardCanvas.ActualHeight;

        _gameController.UpdateBoard();
    }
}