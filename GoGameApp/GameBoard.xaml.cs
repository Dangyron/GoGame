using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoGame.Utility;
using GoGame.Utility.Constants;
using GoGame.Utility.Helpers;
using GoGameApp.Models;

namespace GoGameApp;

public partial class GameBoard
{
    private readonly Board _board;
    
    private StonesStates _stonesStates = StonesStates.Black;
    private Stone _stone;
    public GameBoard()
    {
        InitializeComponent();
        BoardCanvas.Background = Constants.BoardBackGroundColour;
        _board = new Board(BoardCanvas);
        
         _stone = new Stone(_stonesStates);
        
    }

    private void ChangeWindowSize(object sender, SizeChangedEventArgs e)
    {
        if (sender is not GameBoard currentWindow)
            return;

        Constants.WindowWidth = BoardCanvas.ActualWidth;
        Constants.WindowHeight = BoardCanvas.ActualHeight;
        
        UpdateBoard();
    }
    private void UpdateBoard()
    {
        BoardCanvas.Children.Clear();
        _board.Draw();
    }
    private void BoardCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var mousePosition = e.GetPosition(BoardCanvas);
        if (!(mousePosition.X > Constants.BoardHorizontalMargin - Constants.StoneSize)
            || !(mousePosition.X < Constants.BoardHorizontalMargin + Constants.StoneSize + Constants.GridLineLength)) return;

        if (!_board.AddStone(_stone, mousePosition)) return;
        
        _stonesStates = _stonesStates.GetNextColour();
        _stone = new Stone(_stonesStates);
       
        UpdateBoard();
    }
}