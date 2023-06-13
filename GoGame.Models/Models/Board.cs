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
    public Canvas BoardCanvas { get; }

    public Dictionary<StonesStates, int> Score => _stones.ComputingScore();
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new BoardReadWriter().ReadFromFile();
        _groups = new List<StonesGroup>();
    }

    public bool ContainsStoneAtThisPosition(Point position)
    {
        var indexer = position.ConvertPositionToIndexers();

        if (indexer.Equals(Constants.UndefinedIndexer))
            return false;

        return _stones[indexer].StoneStates != StonesStates.Empty;
    }

    public static void DrawTestBoard(int x, int y, int countOfCells, Canvas testBoard)
    {
        double currLineLength = (countOfCells - 1) * Constants.CellSize;
        for (var i = 0; i < countOfCells; i++)
        {
            var verticalLine = new Line
            {
                X1 = i * Constants.CellSize + y,
                Y1 = y,
                X2 = i * Constants.CellSize + x,
                Y2 = currLineLength + y,
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            testBoard.Children.Add(verticalLine);

            var horizontalLine = new Line
            {
                X1 = x,
                Y1 = i * Constants.CellSize + y,
                X2 = currLineLength + x,
                Y2 = i * Constants.CellSize + y,
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            testBoard.Children.Add(horizontalLine);
        }

        var stone = new Stone(StonesStates.Black);
        Canvas.SetTop(stone.Imagination, y - Constants.StoneSize / 2.0);
        Canvas.SetLeft(stone.Imagination, x - Constants.StoneSize / 2.0);
        testBoard.Children.Add(stone.Imagination);
        
        stone = new Stone(StonesStates.White);
        Canvas.SetTop(stone.Imagination, y + currLineLength - Constants.StoneSize / 2.0);
        Canvas.SetLeft(stone.Imagination, x + currLineLength - Constants.StoneSize / 2.0);
        testBoard.Children.Add(stone.Imagination);
    }
    public void Draw()
    {
        DrawBoardBorders();
        DrawBoardStars();
        DrawStones();
        DrawCoordinates();
    }

    private void DrawCoordinates()
    {
        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            var label = new Label { Content = $"{(char)(i + 'A')}", FontSize = 14};
            Canvas.SetTop(label, Constants.BoardVerticalMargin - 30);
            Canvas.SetLeft(label, Constants.BoardHorizontalMargin + i * Constants.CellSize - 8);
            BoardCanvas.Children.Add(label);
            label = new Label { Content = $"{i + 1}", FontSize = 14 };
            Canvas.SetLeft(label, Constants.BoardHorizontalMargin - 25);
            Canvas.SetTop(label, Constants.BoardVerticalMargin + i * Constants.CellSize - 13);
            BoardCanvas.Children.Add(label);
        }
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

            foreach (var group in _groups.CaptureGroups(_stones, position))
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
        var star = BoardHelper.NewStar();
        int offset = Constants.CountOfCells / 2;
        
        Canvas.SetLeft(star,
            offset * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        
        Canvas.SetTop(star,
            offset * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);

        BoardCanvas.Children.Add(star);
    }

    private void DrawInternalStars()
    {
        int center = Constants.CountOfCells / 2;
        int offset = Constants.CountOfCells > 10 ? 3 : 2;

        if (2 * offset > center)
            return;
        
        DrawStar(BoardHelper.NewStar(), Canvas.SetTop, Canvas.SetLeft, center, offset);
        DrawStar(BoardHelper.NewStar(), Canvas.SetBottom, Canvas.SetRight, center, offset);
        DrawStar(BoardHelper.NewStar(), Canvas.SetBottom, Canvas.SetLeft, offset, center);
        DrawStar(BoardHelper.NewStar(), Canvas.SetTop, Canvas.SetRight, offset, center);
    }

    private void DrawCornerStars()
    {
        int offset = Constants.CountOfCells > 10 ? 3 : 2;
        DrawStar(BoardHelper.NewStar(), Canvas.SetTop, Canvas.SetLeft, offset, offset);
        DrawStar(BoardHelper.NewStar(), Canvas.SetTop, Canvas.SetRight, offset, offset);
        DrawStar(BoardHelper.NewStar(), Canvas.SetBottom, Canvas.SetLeft, offset, offset);
        DrawStar(BoardHelper.NewStar(), Canvas.SetBottom, Canvas.SetRight, offset, offset);
    }

    private void DrawStar(Ellipse starPoint, Action<UIElement, double> vertical, Action<UIElement, double> horizontal, int vertOffset, int horzOffset)
    {
        horizontal(starPoint,
            horzOffset * Constants.CellSize + Constants.BoardHorizontalMargin -
            Constants.BoardStarSize / 2.0);
        vertical(starPoint,
            vertOffset * Constants.CellSize + Constants.BoardVerticalMargin -
            Constants.BoardStarSize / 2.0);
        BoardCanvas.Children.Add(starPoint);
    }
}