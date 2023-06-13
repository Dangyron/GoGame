using System.Windows.Media;
using GoGame.Models.Helpers;
using GoGame.Models.Models;
using GoGame.Models.ReadWriters;
using GoGameApp.Windows;

namespace GoGameApp.Pages;

public partial class SettingsPage
{
    private static readonly SettingsReadWriter SettingsReadWriter = new();
    private static SettingDto _settingDto = new();
    private int _size = -1;

    public SettingsPage()
    {
        InitializeComponent();
        _settingDto = SettingsReadWriter.ReadFromFile();
        LineColourChooser.Text = _settingDto.LineColour;
        BackColourChooser.Text = _settingDto.BoardColour;
        CheckNeededComboItem();
        Board.DrawTestBoard(10, 10, 3, BoardExample);
    }

    private void CheckNeededComboItem()
    {
        var items = BoardSizeChooser.Items;
        var currSize = SettingsReadWriter.GetCountOfCellsFromFile().ToString();
        foreach (var item in items)
        {
            var content = (item as ComboBoxItem)?.Content.ToString();
            if (content?.StartsWith(currSize) ?? false)
            {
                BoardSizeChooser.SelectedItem = item;
                return;
            }
        }
    }

    private void LineColourChooser_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (LineColourChooser.IsMaskCompleted)
        {
            if (IsValidColour(LineColourChooser.Text))
            {
                _settingDto.LineColour = LineColourChooser.Text;
                SettingsReadWriter.WriteToFile(_settingDto);
                Constants.BoardStrokeColour = ApplyColour(LineColourChooser.Text);
                DrawTestBoard();
            }
        }
    }

    public static Brush ApplyColour(string colourFrom)
    {
        var rgb = colourFrom.Split(',').Select(i => Convert.ToByte(i)).ToArray();
        return new SolidColorBrush(Color.FromRgb(rgb[0], rgb[1], rgb[2]));
    }

    private void DrawTestBoard()
    {
        BoardExample.Children.Clear();
        Board.DrawTestBoard(10, 10, 3, BoardExample);
    }

    private void BackColourChooser_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BackColourChooser.IsMaskCompleted)
        {
            if (IsValidColour(BackColourChooser.Text))
            {
                _settingDto.BoardColour = BackColourChooser.Text;
                SettingsReadWriter.WriteToFile(_settingDto);
                Constants.BoardBackGroundColour = ApplyColour(BackColourChooser.Text);
                Background = Constants.BoardBackGroundColour;
            }
        }
    }

    private bool IsValidColour(string text)
    {
        var colours = text.Split(',').ToList();

        foreach (var colour in colours)
        {
            int i = 0;
            while (i < colour.Length && !char.IsDigit(colour[i]))
            {
                i++;
            }

            while (i < colour.Length && char.IsDigit(colour[i]))
            {
                i++;
            }

            if (i != colour.Length)
                return false;

            if (!int.TryParse(colour, out var number))
            {
                return false;
            }

            if (number is < 0 or > 255)
            {
                return false;
            }
        }

        return true;
    }

    private void BoardSizeChooser_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = BoardSizeChooser.SelectedItem as ComboBoxItem;
        if (int.TryParse(item!.Content.ToString()!.Split('x')[0], out var size))
        {
            if (_size == -1)
            {
                _size = size;
            }
            else
            {
                _size = size;
                ShowWarning(_size != SettingsReadWriter.GetCountOfCellsFromFile());
            }
        }
    }

    private void ShowWarning(bool b)
    {
        TextWarning.Visibility = b ? Visibility.Visible : Visibility.Hidden;
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var radio = (RadioButton)sender;

        _settingDto.StoneType = radio.Content.ToString()!.ToLower();
        SettingsReadWriter.WriteToFile(_settingDto);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        bool isNeedToClear = StartUpWindow.IsFirstLoad;
        if (_size != -1 && _size != SettingsReadWriter.GetCountOfCellsFromFile())
        {
            Constants.CountOfCells = _size;
            _settingDto.BoardSize = _size;
            SettingsReadWriter.WriteToFile(_settingDto);
            ReadWriteHelper.ClearAllData();
            isNeedToClear = true;
            StartUpWindow.IsFirstLoad = true;
        }

        if (Application.Current.MainWindow is not StartUpWindow mainWindow)
            return;

        var page = new StartUpPage(isNeedToClear);
        mainWindow.Content = page;
        mainWindow.BindAllButtons(page);
        mainWindow.Show();
    }
}