using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGameApp;

public class ToolBox
{
    private readonly Canvas _boardCanvas;
    private readonly Window _thisWindow;
    private WindowState _previousState;
    public ToolBox(Canvas boardCanvas, Window thisWindow)
    {
        _boardCanvas = boardCanvas;
        _thisWindow = thisWindow;
    }
    
    public void SetToolBox()
    {
        DrawCross();
        DrawMinimizeWindow();
        DrawMaximizeWindow();
    }

    private void DrawMaximizeWindow()
    {
        Image maximize = new Image
        {
            Name = Constants.MaximizeName,
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri(_thisWindow.WindowState == WindowState.Maximized ? Constants.MinimizePath : Constants.MaximizePath)),
        };
        
        maximize.MouseEnter += HandleIfMouseEnterImage;
        maximize.MouseLeave += HandleIfMouseLeaveImage;
        
        Canvas.SetRight(maximize, Constants.MaximizeImageRightPosition);
        Canvas.SetTop(maximize, Constants.ToolBoxTopPosition);
        
        _boardCanvas.Children.Add(maximize);
    }

    private void DrawMinimizeWindow()
    {
        var minus = new Image
        {
            Name = Constants.MinusName,
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri(Constants.MinusPath)),
        };
        
        minus.MouseEnter += HandleIfMouseEnterImage;
        minus.MouseLeave += HandleIfMouseLeaveImage;
        minus.MouseDown += HandleIfMouseclickOnMinus;
        
        Canvas.SetRight(minus, Constants.MinimizeImageRightPosition);
        Canvas.SetTop(minus, Constants.ToolBoxTopPosition);
        
        _boardCanvas.Children.Add(minus);
    }

    private void HandleIfMouseclickOnMinus(object sender, MouseButtonEventArgs e)
    {
        switch (_thisWindow.WindowState)
        {
            case WindowState.Maximized or WindowState.Normal:
                _previousState = _thisWindow.WindowState;
                _thisWindow.WindowState = WindowState.Minimized;
                break;
            case WindowState.Minimized:
                _thisWindow.WindowState = _previousState;
                break;
        }
    }

    private void DrawCross()
    {
        var cross = new Image
        {
            Name = Constants.CrossName,
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri(Constants.CrossPath))
        };

        cross.MouseEnter += HandleIfMouseEnterImage;
        cross.MouseLeave += HandleIfMouseLeaveImage;
        cross.MouseDown += HandleIfMouseClickOnCross;
        
        Canvas.SetRight(cross, Constants.CrossImageRightPosition);
        Canvas.SetTop(cross, Constants.ToolBoxTopPosition);
        
        _boardCanvas.Children.Add(cross);
    }

    private void HandleIfMouseClickOnCross(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is Image { Name: "Cross" })
        {
            Application.Current.Shutdown();
        }
    }

    private void HandleIfMouseEnterImage(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not Image image) return;
        
        image.Width = Constants.ToolBoxActiveElementsSize;
        image.Height = Constants.ToolBoxActiveElementsSize;
    }
    
    private void HandleIfMouseLeaveImage(object sender, MouseEventArgs e)
    {
        if (e.OriginalSource is not Image image) return;
        
        image.Width = Constants.ToolBoxElementsSize;
        image.Height = Constants.ToolBoxElementsSize;
    }
}