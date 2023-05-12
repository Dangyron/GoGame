using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GoGame.Utility;

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
        Image minus = new Image
        {
            Name = "Maximize",
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri(_thisWindow.WindowState == WindowState.Maximized ? Constants.MinimizePath : Constants.MaximizePath)),
        };
        
        minus.MouseEnter += HandleIfMouseEnterImage;
        minus.MouseLeave += HandleIfMouseLeaveImage;
        minus.MouseDown += HandleIfMouseclickOnMaximize;
        
        Canvas.SetRight(minus, Constants.MaximizeImageRightPosition);
        Canvas.SetTop(minus, Constants.ToolBoxTopPosition);
        
        _boardCanvas.Children.Add(minus);
    }

    private void HandleIfMouseclickOnMaximize(object sender, MouseButtonEventArgs e)
    {
        _thisWindow.WindowState = _thisWindow.WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            WindowState.Normal => WindowState.Maximized,
            _ => _thisWindow.WindowState
        };
    }

    private void DrawMinimizeWindow()
    {
        var minus = new Image
        {
            Name = "Minus",
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
            Name = "Cross",
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