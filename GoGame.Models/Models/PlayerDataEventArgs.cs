namespace GoGame.Models.Models;

public class PlayerDataEventArgs : EventArgs
{
    public string Player1Name { get; }
    public string Player2Name { get; }
    public string ChosenColor { get; }

    public PlayerDataEventArgs(string player1Name, string player2Name, string chosenColor)
    {
        Player1Name = player1Name;
        Player2Name = player2Name;
        ChosenColor = chosenColor;
    }
}