using GoGame.Utility;
using GoGameApp.Models;

namespace GoGameApp;

public class GameController
{
    private readonly Canvas _boardCanvas;
    private readonly Board _board;
    private readonly IPlayer _firstPlayer;
    private readonly IPlayer _secondPlayer;
    private bool _isGameOver = false;

    public GameController(Canvas boardCanvas)
    {
        _boardCanvas = boardCanvas;
        _board = new Board(boardCanvas);
        _firstPlayer = new Player(StonesStates.Black, "As", _board);
        _secondPlayer = new Player(StonesStates.White, "As", _board);
    }
    public async Task StartGame(CancellationToken token)
    {
        while (!_isGameOver)
        {
            await _firstPlayer.Move();
            UpdateBoard();
            
            await _secondPlayer.Move();
            UpdateBoard();
        }
    }
    
    public void UpdateBoard()
    {
        _boardCanvas.Children.Clear();
        _board.Draw();
        AddMouses();
        _firstPlayer.Mouse.UpdateStoneSize();
        _secondPlayer.Mouse.UpdateStoneSize();
    }

    private void AddMouses()
    {
        if (_firstPlayer.HasMouse)
            _boardCanvas.Children.Add(_firstPlayer.Mouse);
        
        if (_secondPlayer.HasMouse)
            _boardCanvas.Children.Add(_secondPlayer.Mouse);
    }
}