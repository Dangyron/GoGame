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
    private readonly StonesList _prevStones;
    private readonly StonesList _stones;
    private readonly List<StonesGroup> _groups;
    public event Action<int>? StoneCaptured;
    public Label Label { get; } = new();
    public Canvas BoardCanvas { get; }
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new BoardReadWriter().ReadFromFile();
        _prevStones = new ();
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
        var starPoint = new Ellipse
        {
            Width = Constants.BoardStarSize,
            Height = Constants.BoardStarSize,
            Fill = Constants.BoardStrokeColour
        };

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Canvas.SetLeft(starPoint,
                    3 * Constants.CellSize * (1 + j * 2) + Constants.BoardHorizontalMargin -
                    Constants.BoardStarSize / 2.0);
                Canvas.SetTop(starPoint,
                    3 * Constants.CellSize * (1 + i * 2) + Constants.BoardVerticalMargin -
                    Constants.BoardStarSize / 2.0);
                BoardCanvas.Children.Add(starPoint);
                starPoint = new Ellipse
                {
                    Width = Constants.BoardStarSize,
                    Height = Constants.BoardStarSize,
                    Fill = Constants.BoardStrokeColour
                };
            }
        }
    }
}