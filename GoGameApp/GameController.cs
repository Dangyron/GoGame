using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GoGame.Utility;
using GoGame.Utility.Constants;
using GoGame.Utility.Helpers;
using GoGameApp.Models;

namespace GoGameApp;

public class GameController
{
    private readonly Canvas _boardCanvas;
    private readonly Board _board;
    private readonly IPlayer _firstPlayer;
    private readonly IPlayer _secondPlayer;
    private StonesStates _stonesStates = StonesStates.Black;
    private Stone _stone;
    private Ellipse _ellipse;
    private static bool _isGameOver = false;

    public GameController(Canvas boardCanvas)
    {
        _boardCanvas = boardCanvas;
        _board = new Board(boardCanvas);
        _stone = new Stone(_stonesStates);
        _firstPlayer = new Player(StonesStates.Black, "As", _board);
        _secondPlayer = new Player(StonesStates.White, "As", _board);

        _ellipse = new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = _stonesStates.ConvertStonesColourToMouseBrush()
        };
        
        _boardCanvas.Children.Add(_ellipse);
    }
    public async Task StartGame(CancellationToken token)
    {
        while (!_isGameOver)
        {
            //await _firstPlayer.Move(); 
            //await _secondPlayer.Move();
            _isGameOver = true;
        }
    }
    
    public void GameController_OnMouseMove(object sender, MouseEventArgs e)
    {
        var position = e.GetPosition(_boardCanvas);
        
        var nearest = position.GetNearestPosition();

        bool isInBoard = !nearest.Equals(Constants.UndefinedPoint);
        
        UpdateEllipse(isInBoard);
        
        if (!isInBoard)
            return;
        
        Canvas.SetLeft(_ellipse, nearest.X - Constants.StoneSize / 2.0);
        Canvas.SetTop(_ellipse, nearest.Y - Constants.StoneSize / 2.0);
    }
    
    public void UpdateBoard()
    {
        _ellipse.UpdateSize();
        _stone.Imagination.UpdateSize();
        _boardCanvas.Children.Clear();
        _board.Draw();
        _boardCanvas.Children.Add(_ellipse);
    }
    
    public void BoardCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(_boardCanvas);
        if (!(mousePosition.X > Constants.BoardHorizontalMargin - Constants.StoneSize)
            || !(mousePosition.X < Constants.BoardHorizontalMargin + Constants.StoneSize + Constants.GridLineLength)) return;

        if (!_board.AddStone(_stone, mousePosition)) return;
        
        _stonesStates = _stonesStates.GetNextColour();
        _stone = new Stone(_stonesStates);
        UpdateEllipse(false, true);
        UpdateBoard();
    }

    private void UpdateEllipse(bool isMouseOnBoardGrid, bool isMouseClicked = false)
    {
        if (isMouseClicked)
        {
            _ellipse = new Ellipse
            {
                Width = Constants.StoneSize,
                Height = Constants.StoneSize
            };
        }

        _ellipse.Fill = !isMouseOnBoardGrid
            ? Constants.EmptyStone
            : _stone.StoneStates.ConvertStonesColourToMouseBrush();
    }
}