using System.Windows;
using System.Windows.Controls;
using GoGame.Utility.Constants;

namespace GoGame.Models.Models;

public class BoardInterface
{
    private readonly Canvas _boardCanvas;
    private readonly IPlayer[] _players;
    private readonly Label[] _playersName;
    private readonly Label[] _playersCapturedStones;
    
    public BoardInterface(Canvas boardCanvas, IPlayer[] players)
    {
        _boardCanvas = boardCanvas;
        _players = players;
        _playersName = new[]
        {
            new Label() {Content = players[0].Name, FontSize = 14},
            new Label() {Content = players[1].Name, FontSize = 14}
        };
        
        _playersCapturedStones = new[]
        {
            new Label() {Content = players[0].CapturedStones, FontSize = 14},
            new Label() {Content = players[1].CapturedStones, FontSize = 14}
        };
    }

    public void Draw()
    {
        DrawNames();
        DrawCapturedStones();
        DrawResignButtons();
        DrawTimers();
    }

    private void DrawTimers()
    {
        
    }

    private void DrawResignButtons()
    {
        
    }

    private void DrawCapturedStones()
    {
        _playersCapturedStones[0].Content = $"Captured: {_players[0].CapturedStones}";
        _playersCapturedStones[1].Content = $"Captured: {_players[1].CapturedStones}";
        
        Canvas.SetTop(_playersCapturedStones[0], Constants.BoardVerticalMargin * 2 - 10);
        Canvas.SetBottom(_playersCapturedStones[1], Constants.BoardVerticalMargin * 2 - 10);
        
        Canvas.SetLeft(_playersCapturedStones[0], Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);
        Canvas.SetLeft(_playersCapturedStones[1], Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);

        _boardCanvas.Children.Add(_playersCapturedStones[0]);
        _boardCanvas.Children.Add(_playersCapturedStones[1]);
    }

    private void DrawNames()
    {
        Canvas.SetTop(_playersName[0], Constants.BoardVerticalMargin - 10);
        Canvas.SetBottom(_playersName[1], Constants.BoardVerticalMargin - 10);
        
        Canvas.SetLeft(_playersName[0], Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);
        Canvas.SetLeft(_playersName[1], Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);

        _boardCanvas.Children.Add(_playersName[0]);
        _boardCanvas.Children.Add(_playersName[1]);
    }
}