using System.Windows.Controls;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Width = Constants.WindowWidth;
        Height = Constants.WindowHeight;
        DrawBoard();
    }

    private void DrawBoard()
    {
        BoardCanvas.Background = Constants.BoardBackGroundColour;
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

        DrawBoardStars();
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
                Canvas.SetLeft(starPoint, 3 * Constants.CellSize * (1 + j * 2) + Constants.BoardHorizontalMargin - Constants.BoardStarSize / 2.0);
                Canvas.SetTop(starPoint, 3 * Constants.CellSize * (1 + i * 2) + + Constants.BoardVerticalMargin - Constants.BoardStarSize / 2.0);
                BoardCanvas.Children.Add(starPoint);
                starPoint = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Constants.BoardStrokeColour
                };
            }
        }
    }
}