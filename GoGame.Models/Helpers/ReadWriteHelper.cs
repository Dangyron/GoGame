using GoGame.Models.ReadWriters;

namespace GoGame.Models.Helpers;

public static class ReadWriteHelper
{
    public static void ClearAllData()
    {
        new BoardReadWriter().Clear();
        new PlayerReadWriter().Clear();
        new CapturedStonesReadWriter().Clear();
    }
}