using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp.Models;

public class Board
{
    private readonly List<List<Stone>> _prevStones;
    private readonly List<List<Stone>> _stones;
    public Canvas BoardCanvas { get; }
    public Board(Canvas boardCanvas)
    {
        BoardCanvas = boardCanvas;
        _stones = new List<List<Stone>>(Constants.CountOfCells);
        _prevStones = new List<List<Stone>>(Constants.CountOfCells);
        InitPoints();
    }

    public bool ContainsStoneAtThisPosition(Point position)
    {
        var indexer = position.ConvertPositionToIndexers();

        if (indexer == Constants.UndefinedIndexer)
            return false;
        
        return _stones[indexer.I][indexer.J].StoneStates != StonesStates.Empty;
    }
    
    private void InitPoints()
    {
        for (var i = 0; i < Constants.CountOfCells; i++)
        {
            _stones.Add(new List<Stone>(Constants.CountOfCells));
            _prevStones.Add(new List<Stone>(Constants.CountOfCells));
            
            for (var j = 0; j < Constants.CountOfCells; j++)
            {
                _stones[i].Add(new Stone(StonesStates.Empty));
                _prevStones[i].Add(new Stone(StonesStates.Empty));
            }
        }
    }

    public void Draw()
    {
        DrawBoardBorders();
        DrawBoardStars();
        DrawStones();
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

        if (position.Equals(Constants.UndefinedIndexer))
        {
            return false;
        }
        
        if (_stones[position.I][position.J].StoneStates == StonesStates.Empty)
        {
            var tmp = _stones[position.I][position.J];
            _stones[position.I][position.J] = stone;

            if (_stones == _prevStones)
            {
                _stones[position.I][position.J] = tmp;
                Notification.PositionRepetition();
                return false;
            }

            _prevStones[position.I][position.J] = stone;
            return true;
        }
        Notification.InvalidMove();

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