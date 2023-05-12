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
        
    }

    private void DrawMinimizeWindow()
    {
        Image minus = new Image
        {
            Name = "Minus",
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri("""D:\riderRepos\GoGame\GoGame.Utility\Raw\Images\minus.png""")),
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
        _thisWindow.WindowState = WindowState.Minimized;
    }

    private void DrawCross()
    {
        Image cross = new Image
        {
            Name = "Cross",
            Width = Constants.ToolBoxElementsSize,
            Height = Constants.ToolBoxElementsSize,
            Source = new BitmapImage(new Uri("""D:\riderRepos\GoGame\GoGame.Utility\Raw\Images\cross.png"""))
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