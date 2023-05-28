using System.Windows;
using GoGame.Utility.Constants;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Models.Helpers;
using GoGame.Utility;

namespace GoGame.Models.Models;

public class Board
{
    private readonly StonesList _prevStones;
    private readonly StonesList _stones;
    private readonly List<StonesGroup> _groups;
    public event Action<int>? StoneCaptured;
    
    public Canvas BoardCanvas { get; }
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new ();
        _prevStones = new ();
        _groups = new List<StonesGroup>();
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
    }
    
    private void DrawStones()
    {
        for (int i = 0; i < Utility.Constants.Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Utility.Constants.Constants.CountOfCells; j++)
            {
                var position = new Point
                {
                    X = Utility.Constants.Constants.BoardHorizontalMargin + j * Utility.Constants.Constants.CellSize - Utility.Constants.Constants.StoneSize / 2.0,
                    Y = Utility.Constants.Constants.BoardVerticalMargin + i * Utility.Constants.Constants.CellSize - Utility.Constants.Constants.StoneSize / 2.0
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
            
            return true;
        }

        return false;
    }

    private void DrawBoardBorders()
    {
        for (var i = 0; i < Utility.Constants.Constants.CountOfCells; i++)
        {
            var verticalLine = new Line
            {
                X1 = i * Utility.Constants.Constants.CellSize + Utility.Constants.Constants.BoardHorizontalMargin,
                Y1 = Utility.Constants.Constants.BoardVerticalMargin,
                X2 = i * Utility.Constants.Constants.CellSize + Utility.Constants.Constants.BoardHorizontalMargin,
                Y2 = Utility.Constants.Constants.GridLineLength + Utility.Constants.Constants.BoardVerticalMargin,
                Stroke = Utility.Constants.Constants.BoardStrokeColour,
                StrokeThickness = Utility.Constants.Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(verticalLine);

            var horizontalLine = new Line
            {
                X1 = Utility.Constants.Constants.BoardHorizontalMargin,
                Y1 = i * Utility.Constants.Constants.CellSize + Utility.Constants.Constants.BoardVerticalMargin,
                X2 = Utility.Constants.Constants.GridLineLength + Utility.Constants.Constants.BoardHorizontalMargin,
                Y2 = i * Utility.Constants.Constants.CellSize + Utility.Constants.Constants.BoardVerticalMargin,
                Stroke = Utility.Constants.Constants.BoardStrokeColour,
                StrokeThickness = Utility.Constants.Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(horizontalLine);
        }
    }

    private void DrawBoardStars()
    {
        var starPoint = new Ellipse
        {
            Width = Utility.Constants.Constants.BoardStarSize,
            Height = Utility.Constants.Constants.BoardStarSize,
            Fill = Utility.Constants.Constants.BoardStrokeColour
        };

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Canvas.SetLeft(starPoint,
                    3 * Utility.Constants.Constants.CellSize * (1 + j * 2) + Utility.Constants.Constants.BoardHorizontalMargin -
                    Utility.Constants.Constants.BoardStarSize / 2.0);
                Canvas.SetTop(starPoint,
                    3 * Utility.Constants.Constants.CellSize * (1 + i * 2) + Utility.Constants.Constants.BoardVerticalMargin -
                    Utility.Constants.Constants.BoardStarSize / 2.0);
                BoardCanvas.Children.Add(starPoint);
                starPoint = new Ellipse
                {
                    Width = Utility.Constants.Constants.BoardStarSize,
                    Height = Utility.Constants.Constants.BoardStarSize,
                    Fill = Utility.Constants.Constants.BoardStrokeColour
                };
            }
        }
    }
}