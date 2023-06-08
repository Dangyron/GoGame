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
    public readonly Button[] ResignButtons;

    public BoardInterface(Canvas boardCanvas, IPlayer[] players)
    {
        _boardCanvas = boardCanvas;
        _players = players;
        _playersName = new[]
        {
            new Label() { Content = players[0].Name, FontSize = 14 },
            new Label() { Content = players[1].Name, FontSize = 14 }
        };

        _playersCapturedStones = new[]
        {
            new Label() { Content = players[0].CapturedStones, FontSize = 14 },
            new Label() { Content = players[1].CapturedStones, FontSize = 14 }
        };

        ResignButtons = new[]
        {
            new Button() { Content = "⚑", FontSize = 30, Width = 40 },
            new Button() { Content = "⚑", FontSize = 30, Width = 40 },
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
        PlaceUiToTop(ResignButtons[0], 2);
        PlaceUiToBottom(ResignButtons[1], 2);
    }

    private void DrawCapturedStones()
    {
        _playersCapturedStones[0].Content = $"Captured: {_players[0].CapturedStones}";
        _playersCapturedStones[1].Content = $"Captured: {_players[1].CapturedStones}";
        PlaceUiToTop(_playersCapturedStones[0], 1);
        PlaceUiToBottom(_playersCapturedStones[1], 1);
    }

    private void DrawNames()
    {
        PlaceUiToTop(_playersName[0], 0);
        PlaceUiToBottom(_playersName[1], 0);
    }

    private void PlaceUiToTop(UIElement element, int offset)
    {
        Canvas.SetTop(element, Constants.BoardVerticalMargin + Constants.CellSize * offset - 10);
        Canvas.SetLeft(element, Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);

        _boardCanvas.Children.Add(element);
    }

    private void PlaceUiToBottom(UIElement element, int offset)
    {
        Canvas.SetBottom(element, Constants.BoardVerticalMargin + Constants.CellSize * offset - 10);
        Canvas.SetLeft(element, Constants.BoardHorizontalMargin + Constants.GridLineLength + 10);

        _boardCanvas.Children.Add(element);
    }
}