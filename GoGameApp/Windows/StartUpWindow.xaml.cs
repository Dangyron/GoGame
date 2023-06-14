using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGameApp.Pages;

namespace GoGameApp.Windows;

public partial class StartUpWindow
{
    private static GameBoardWindow? _gameBoard;
    public static bool IsGameLoaded = false;
    private static PlayerDataEventArgs? _playerDataEventArgs;
    
    public StartUpWindow()
    {
        CheckIfGameLoaded();
        UpdateConstants();

        InitializeComponent();
        Constants.BoardBackGroundColour = SettingsPage.ApplyColour(SettingsReadWriter.GetBackColourOfBoard());
        Constants.BoardStrokeColour = SettingsPage.ApplyColour(SettingsReadWriter.GetLineColourOfBoard());
        Background = Constants.BoardBackGroundColour;
        var startUpPage = new StartUpPage(IsGameLoaded);
        Content = startUpPage;
        BindAllButtons(startUpPage);
    }

    private void CheckIfGameLoaded()
    {
        IsGameLoaded = new LoadReadWriter().ReadFromFile();
    }

    private void UpdateConstants()
    {
        Constants.CountOfCells = SettingsReadWriter.GetCountOfCellsFromFile();
        Constants.BoardBackGroundColour = SettingsPage.ApplyColour(SettingsReadWriter.GetBackColourOfBoard());
        
        Background = Constants.BoardBackGroundColour;
    }

    public void BindAllButtons(StartUpPage startUpPage)
    {
        startUpPage.Button0.Click += ButtonBase0_OnClick;
        startUpPage.ShowContinue(!IsGameLoaded);
        startUpPage.Button1.Click += ButtonBase1_OnClick;
        startUpPage.Button2.Click += ButtonBase2_OnClick;
        startUpPage.Button3.Click += ButtonBase3_OnClick;
        startUpPage.Button4.Click += ButtonBase4_OnClick;
        UpdateConstants();
    }

    private void ButtonBase4_OnClick(object sender, RoutedEventArgs e)
    {
        Content = new AdditionalInfoPage();
    }

    private void ButtonBase0_OnClick(object sender, RoutedEventArgs e)
    {
        if (_gameBoard is null)
        {
            if (_playerDataEventArgs is null)
                _playerDataEventArgs = new PlayerInfoReadWriter().ReadFromFile();
            _gameBoard = new GameBoardWindow(_playerDataEventArgs);
        }
        Content = _gameBoard;
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
        if (!IsGameLoaded)
        {
            var prompt = new StartGame();
            prompt.PlayerDataSubmitted += MainWindow_PlayerDataSubmitted;
            prompt.ShowDialog();
        }
        else
        {
            var res = MessageBox.Show(this, "Are you sure, that you want to start new game", "New game",
                MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                var prompt = new StartGame();
                prompt.PlayerDataSubmitted += MainWindow_PlayerDataSubmitted;
                prompt.ShowDialog();
            }
        }
    }

    private void MainWindow_PlayerDataSubmitted(object? sender, PlayerDataEventArgs e)
    {
        _playerDataEventArgs = e;
        new PlayerInfoReadWriter().WriteToFile(e);
        ReadWriteHelper.ClearAllData();
        _gameBoard = new GameBoardWindow(_playerDataEventArgs);
        Content = _gameBoard;
        IsGameLoaded = true;
        new LoadReadWriter().WriteToFile(true);
    }

    private void StartUpWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (_gameBoard is null)
            return;

        var size = new Size(ActualWidth, ActualHeight);

        _gameBoard.ChangeBoardSize(size);
    }
}