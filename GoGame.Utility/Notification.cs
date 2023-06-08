using System.Windows.Forms;

namespace GoGame.Utility;

public static class Notification
{
    public static void InvalidMove()
    {
        MessageBox.Show("Invalid move, this position is already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    
    public static void Ko()
    {
        MessageBox.Show("The game would be repeating with that move, please play somewhere else first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void SelfCaptured()
    {
        MessageBox.Show("Self-capture is not allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void CongratulationWinner(string name, StonesStates colour)
    {
        MessageBox.Show($"{name} ({colour}) win", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void ShowScore(Dictionary<StonesStates, int> score)
    {
        string res = string.Empty;
        foreach (var stone in score)
        {
            res += $"{stone.Key} {stone.Value}\n";
        }
        MessageBox.Show(res, "score", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}