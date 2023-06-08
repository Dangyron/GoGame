﻿using System.Windows.Media;
using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGameApp.Pages;

namespace GoGameApp.Windows;

public partial class StartUpWindow
{
    private GameBoardWindow? _gameBoard;
    public static bool _isFirstLoad = true;
    private PlayerDataEventArgs _playerDataEventArgs;
    
    public StartUpWindow()
    {
        if (_isFirstLoad)
        {
            ReadWriteHelper.ClearAllData();
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
        startUpPage.Button3.Click += ButtonBase3_OnClick;
    }

    private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isFirstLoad)
        {
            var prompt = new StartGame();
            prompt.PlayerDataSubmitted += MainWindow_PlayerDataSubmitted;
            prompt.ShowDialog();
            _isFirstLoad = false;
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