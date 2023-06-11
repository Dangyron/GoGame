using System.Windows.Media;
using GoGame.Models.Models;

namespace GoGameApp.Windows;

public partial class StartGame
{
    public event EventHandler<PlayerDataEventArgs>? PlayerDataSubmitted;

    public StartGame()
    {
        InitializeComponent();
    }

    private void ColorButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var name1 = Player1Name.Text.Trim();
        var name2 = Player2Name.Text.Trim();

        if (name1.Length < 1 || name2.Length < 1)
        {
            return;
        }
        
        OnPlayerDataSubmitted(new PlayerDataEventArgs(name1, name2, button!.Content.ToString()!));
        Close();
    }
    
    private void OnPlayerDataSubmitted(PlayerDataEventArgs e)
    {
        PlayerDataSubmitted?.Invoke(this, e);
    }

    private void Player1Name_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var text = sender as TextBox;
        Player1Name.BorderBrush = text?.Text.Length < 1 ? Brushes.Crimson : Brushes.Gray;
    }
}