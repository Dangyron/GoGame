using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGameApp.Pages;

namespace GoGameApp.Windows;

public partial class StartUpWindow
{
    private GameBoardWindow? _gameBoard;
    public static bool IsFirstLoad = true;
    private static PlayerDataEventArgs? _playerDataEventArgs;
    
    public StartUpWindow()
    {
        if (IsFirstLoad)
        {
            ReadWriteHelper.ClearAllData();
            Constants.CountOfCells = SettingsReadWriter.GetCountOfCellsFromFile();
        }

        InitializeComponent();
        Constants.BoardBackGroundColour = SettingsPage.ApplyColour(SettingsReadWriter.GetBackColourOfBoard());
        Constants.BoardStrokeColour = SettingsPage.ApplyColour(SettingsReadWriter.GetLineColourOfBoard());
        Background = Constants.BoardBackGroundColour;
        var startUpPage = new StartUpPage(IsFirstLoad);
        Content = startUpPage;
        BindAllButtons(startUpPage);
    }

    public void BindAllButtons(StartUpPage startUpPage)
    {
        startUpPage.Button1.Click += ButtonBase1_OnClick;
        startUpPage.Button2.Click += ButtonBase2_OnClick;
        startUpPage.Button3.Click += ButtonBase3_OnClick;
        Background = Constants.BoardBackGroundColour;
    }

    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
        Content = new SettingsPage();
    }

    private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
    {
        if (_playerDataEventArgs is null || IsFirstLoad)
        {
            var prompt = new StartGame();
            prompt.PlayerDataSubmitted += MainWindow_PlayerDataSubmitted;
            prompt.ShowDialog();
            IsFirstLoad = false;
        }
        else
        {
            _gameBoard = new GameBoardWindow(_playerDataEventArgs);
            Content = _gameBoard;
        }
    }

    private void MainWindow_PlayerDataSubmitted(object? sender, PlayerDataEventArgs e)
    {
        _playerDataEventArgs = e;
        _gameBoard = new GameBoardWindow(_playerDataEventArgs);
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