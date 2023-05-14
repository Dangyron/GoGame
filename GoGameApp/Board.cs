using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGameApp;

public class Board
{
    private readonly List<List<Stone>> _stones;
    private readonly Canvas _boardCanvas;
    public Board(Canvas boardCanvas)
    {
        _boardCanvas = boardCanvas;
        _stones = new List<List<Stone>>(Constants.CountOfCells);
        
        InitPoints();
    }

    private void InitPoints()
    {
        var keys = StonesHelper.GetAllPossiblePoints();
        for (var i = 0; i < Constants.CountOfCells; i++)
        {
            _stones.Add(new List<Stone>(Constants.CountOfCells));
            for (var j = 0; j < Constants.CountOfCells; j++)
            {
                _stones[i].Add(new Stone(StonesStates.Empty));
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
                Canvas.SetLeft(_stones[i][j].Imagination, position.X);
                Canvas.SetTop(_stones[i][j].Imagination, position.Y);
                _boardCanvas.Children.Add(_stones[i][j].Imagination);
            }
        }
    }

    public bool AddStone(Stone stone, Point point)
    {
        StoneIndexer position;
        try
        {
            position = point.ConvertPositionToIndexers();
        }
        catch (Exception e)
        {
            return false;
        }
        

        if (_stones[position.I][position.J].StoneStates == StonesStates.Empty)
        {
            _stones[position.I][position.J] = stone;
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
                Y1 = Math.Max(Constants.BoardVerticalMargin, Constants.ToolBoxActiveElementsSize),
                X2 = i * Constants.CellSize + Constants.BoardHorizontalMargin,
                Y2 = Constants.GridLineLength + Math.Max(Constants.BoardVerticalMargin, Constants.ToolBoxActiveElementsSize),
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            _boardCanvas.Children.Add(verticalLine);

            var horizontalLine = new Line
            {
                X1 = Constants.BoardHorizontalMargin,
                Y1 = i * Constants.CellSize + Math.Max(Constants.BoardVerticalMargin, Constants.ToolBoxActiveElementsSize),
                X2 = Constants.GridLineLength + Constants.BoardHorizontalMargin,
                Y2 = i * Constants.CellSize + Math.Max(Constants.BoardVerticalMargin, Constants.ToolBoxActiveElementsSize),
                Stroke = Constants.BoardStrokeColour,
                StrokeThickness = Constants.BoardStrokeThickness
            };
            _boardCanvas.Children.Add(horizontalLine);
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
                    3 * Constants.CellSize * (1 + i * 2) + +Constants.BoardVerticalMargin -
                    Constants.BoardStarSize / 2.0);
                _boardCanvas.Children.Add(starPoint);
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