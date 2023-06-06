using System.Windows.Media;
using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGameApp.Pages;

namespace GoGameApp.Windows;

public partial class StartUpWindow
{
    private GameBoard? _gameBoard;
    private SettingsPage? _settingsPage;
    private static bool _isFirstLoad = true;
    public StartUpWindow()
    {
        if (_isFirstLoad)
        {
            new BoardReadWriter().Clear();
            new PlayerReadWriter().Clear();
            new CapturedStonesReadWriter().Clear();
            _isFirstLoad = false;
        }
        InitializeComponent();
        Background = Constants.BoardBackGroundColour;
        var startUpPage = new StartUpPage();
        Content = startUpPage;
        BindAllButtons(startUpPage);
    }

    public void BindAllButtons(StartUpPage startUpPage)
    {
        startUpPage.Button1.Click += ButtonBase1_OnClick;
        startUpPage.Button2.Click += ButtonBase2_OnClick;
        startUpPage.Button3.Click += ButtonBase3_OnClick;
    }
    
    private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
        _settingsPage = new SettingsPage();
        Content = _settingsPage;
    }

    private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
    {
        _gameBoard = new GameBoard();
        Content = _gameBoard;
    }

    private void StartUpWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_gameBoard is null)
            return;

        var size = new Size(ActualWidth, ActualHeight);
        
        _gameBoard.ChangeBoardSize(size);
    }
    
}