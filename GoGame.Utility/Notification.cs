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

    public static void CongratulationWinner(string[] names, Dictionary<StonesStates, int> score)
    {
        var res = string.Empty;

        double first = score[StonesStates.Black];
        double second = score[StonesStates.White];

        res += "Black: " + names[0] + " score: " + first + "\n";
        res += "White: " + names[1] + " score: " + second + " komi:" + Constants.Constants.Komi;
        (string, StonesStates) winner = first > second + Constants.Constants.Komi
            ? (names[0], StonesStates.Black)
            : (names[1], StonesStates.White);
        res += "\n\n";
        res += $"Win: {winner.Item1} ({winner.Item2})";
        MessageBox.Show($"{res}", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
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