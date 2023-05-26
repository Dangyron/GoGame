using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Utility.Helpers;

namespace GoGame.Utility.Models;

public class Board
{
    private readonly StonesList _prevStones;
    private readonly StonesList _stones;
    public Canvas BoardCanvas { get; }
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new ();
        _prevStones = new ();
    }

    public bool ContainsStoneAtThisPosition(Point position)
    {
        var indexer = position.ConvertPositionToIndexers();

        if (indexer.Equals(Constants.Constants.UndefinedIndexer))
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
        for (int i = 0; i < Constants.Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.Constants.CountOfCells; j++)
            {
                var position = new Point
                {
                    X = Constants.Constants.BoardHorizontalMargin + j * Constants.Constants.CellSize - Constants.Constants.StoneSize / 2.0,
                    Y = Constants.Constants.BoardVerticalMargin + i * Constants.Constants.CellSize - Constants.Constants.StoneSize / 2.0
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
        
        if (_stones.CheckAllRules(stone, position, _prevStones))
        {
            _stones[position.I][position.J] = stone;

            _prevStones[position.I][position.J] = stone;
            return true;
        }

        return false;
    }

    private void DrawBoardBorders()
    {
        for (var i = 0; i < Constants.Constants.CountOfCells; i++)
        {
            var verticalLine = new Line
            {
                X1 = i * Constants.Constants.CellSize + Constants.Constants.BoardHorizontalMargin,
                Y1 = Constants.Constants.BoardVerticalMargin,
                X2 = i * Constants.Constants.CellSize + Constants.Constants.BoardHorizontalMargin,
                Y2 = Constants.Constants.GridLineLength + Constants.Constants.BoardVerticalMargin,
                Stroke = Constants.Constants.BoardStrokeColour,
                StrokeThickness = Constants.Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(verticalLine);

            var horizontalLine = new Line
            {
                X1 = Constants.Constants.BoardHorizontalMargin,
                Y1 = i * Constants.Constants.CellSize + Constants.Constants.BoardVerticalMargin,
                X2 = Constants.Constants.GridLineLength + Constants.Constants.BoardHorizontalMargin,
                Y2 = i * Constants.Constants.CellSize + Constants.Constants.BoardVerticalMargin,
                Stroke = Constants.Constants.BoardStrokeColour,
                StrokeThickness = Constants.Constants.BoardStrokeThickness
            };
            BoardCanvas.Children.Add(horizontalLine);
        }
    }

    private void DrawBoardStars()
    {
        var starPoint = new Ellipse
        {
            Width = Constants.Constants.BoardStarSize,
            Height = Constants.Constants.BoardStarSize,
            Fill = Constants.Constants.BoardStrokeColour
        };

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Canvas.SetLeft(starPoint,
                    3 * Constants.Constants.CellSize * (1 + j * 2) + Constants.Constants.BoardHorizontalMargin -
                    Constants.Constants.BoardStarSize / 2.0);
                Canvas.SetTop(starPoint,
                    3 * Constants.Constants.CellSize * (1 + i * 2) + Constants.Constants.BoardVerticalMargin -
                    Constants.Constants.BoardStarSize / 2.0);
                BoardCanvas.Children.Add(starPoint);
                starPoint = new Ellipse
                {
                    Width = Constants.Constants.BoardStarSize,
                    Height = Constants.Constants.BoardStarSize,
                    Fill = Constants.Constants.BoardStrokeColour
                };
            }
        }
    }
}