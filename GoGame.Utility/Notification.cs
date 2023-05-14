using System.Windows.Forms;

namespace GoGame.Utility;

public static class Notification
{
    public static void InvalidMove()
    {
        MessageBox.Show("Invalid move, this position is already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}