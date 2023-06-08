using GoGame.Models.Models;

namespace GoGameApp.Windows;

public partial class StartGame
{
    public event EventHandler<PlayerDataEventArgs> PlayerDataSubmitted;
    
    public StartGame()
    {
        InitializeComponent();
    }

    private void ColorButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        OnPlayerDataSubmitted(new PlayerDataEventArgs(Player1Name.Text, Player2Name.Text, button!.Content.ToString()));
        Close();
    }
    
    private void OnPlayerDataSubmitted(PlayerDataEventArgs e)
    {
        PlayerDataSubmitted?.Invoke(this, e);
    }
}