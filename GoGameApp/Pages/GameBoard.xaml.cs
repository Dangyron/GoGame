namespace GoGameApp.Pages;

public partial class GameBoard
{
    private readonly GameController _gameController;
    public GameBoard()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _gameController = new GameController(BoardCanvas, this);
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