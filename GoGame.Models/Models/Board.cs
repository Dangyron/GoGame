using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Models.Helpers;
using GoGame.Models.ReadWriters;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.Models;

public class Board
{
    private readonly StonesList _stones;
    private readonly List<StonesGroup> _groups;
    public event Action<int>? StoneCaptured;
    public Label Label { get; } = new();
    public Canvas BoardCanvas { get; }

    public Dictionary<StonesStates, int> Score => _stones.ComputingScore(_groups);
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new BoardReadWriter().ReadFromFile();
        _groups = new List<StonesGroup>();

        Canvas.SetTop(Label, 10);
        Canvas.SetRight(Label, 10);
    }

    public bool ContainsStoneAtThisPosition(Point position)
    {
        var indexer = position.ConvertPositionToIndexers();

        if (indexer.Equals(Constants.UndefinedIndexer))
            return false;

        return _stones[indexer].StoneStates != StonesStates.Empty;
    }

    public void Draw()
    {
        DrawBoardBorders();
        DrawBoardStars();
        DrawStones();
        BoardCanvas.Children.Add(Label);
    }

    private void DrawStones()
    {
        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                var position = new Point
                {
                    X = Constants.BoardHorizontalMargin + j * Constants.CellSize - Constants.StoneSize / 2.0,
                    Y = Constants.BoardVerticalMargin + i * Constants.CellSize - Constants.StoneSize / 2.0
                };
                _stones[i][j].Imagination.UpdateStoneSize();

                Canvas.SetLeft(_stones[i][j].Imagination, position.X);
                Canvas.SetTop(_stones[i][j].Imagination, position.Y);
                BoardCanvas.Children.Add(_stones[i][j].Imagination);
            }
        }
    }

    public bool AddStone(Stone stone, Point point)
    {
        var position = point.ConvertPositionToIndexers();

        if (_stones.CheckPlacingRules(stone, position))
        {
            if (!_stones.TryAddToGroup(_groups, position))
                return false;

            foreach (var group in _groups.CaptureStones(_stones, stone.StoneStates, position))
            {
                StoneCaptured?.Invoke(group.Count);
                _stones.RemoveGroup(group);
            }

            _groups.RemoveAll(i => i.Count == 0);

            new BoardReadWriter().WriteToFile(_stones);
            return true;
        }

        return false;
    }

    private void DrawBoardBorders()
    {
        for (var i = 0; i < Constants.CountOfCells; i++)
        {
            var verticalLine = new Line
            {
                X1 = i * Constants.CellSize + Constants.BoardHorizontalMargin,
                Y1 = Constants.BoardVerticalMargin,
                X2 = i * Constants.CellSize + Constants.BoardHorizontalMargin,
                Y2 = Constants.GridLineLength + Constants.BoardVerticalMargin,
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(verticalLine);

            var horizontalLine = new Line
            {
                X1 = Constants.BoardHorizontalMargin,
                Y1 = i * Constants.CellSize + Constants.BoardVerticalMargin,
                X2 = Constants.GridLineLength + Constants.BoardHorizontalMargin,
                Y2 = i * Constants.CellSize + Constants.BoardVerticalMargin,
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(horizontalLine);
        }
    }

    private void DrawBoardStars()
    {
        DrawCornerStars();
        DrawInternalStars();
        DrawCenterStar();
    }

    private void DrawCenterStar()
    {
        var star = BoardHelper.RenewStar();
        
        Canvas.SetLeft(star,
            9 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        
        Canvas.SetTop(star,
            9 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);

        BoardCanvas.Children.Add(star);
    }

    private void DrawInternalStars()
    {
        var star = BoardHelper.RenewStar();
        
        Canvas.SetLeft(star,
            9 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        
        Canvas.SetTop(star,
            3 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        
        BoardCanvas.Children.Add(star);
        
        star = BoardHelper.RenewStar();
        
        Canvas.SetLeft(star,
            9 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        
        Canvas.SetBottom(star,
            3 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        
        BoardCanvas.Children.Add(star);
        
        star = BoardHelper.RenewStar();
        
        Canvas.SetLeft(star,
            3 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
         
        Canvas.SetBottom(star,
            9 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        
        BoardCanvas.Children.Add(star);
        
        star = BoardHelper.RenewStar();
        
        Canvas.SetRight(star,
            3 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        
        Canvas.SetTop(star,
            9 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        
        BoardCanvas.Children.Add(star);
    }

    private void DrawCornerStars()
    {
        DrawStar(BoardHelper.RenewStar(), Canvas.SetTop, Canvas.SetLeft);
        DrawStar(BoardHelper.RenewStar(), Canvas.SetTop, Canvas.SetRight);
        DrawStar(BoardHelper.RenewStar(), Canvas.SetBottom, Canvas.SetLeft);
        DrawStar(BoardHelper.RenewStar(), Canvas.SetBottom, Canvas.SetRight);
    }

    private void DrawStar(Ellipse starPoint, Action<UIElement, double> vertical, Action<UIElement, double> horizontal)
    {
        horizontal(starPoint,
            3 * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        vertical(starPoint,
            3 * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        BoardCanvas.Children.Add(starPoint);
    }
}