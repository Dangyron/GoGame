using System.Threading;
using System.Threading.Tasks;

namespace GoGameApp;

public class GameController
{
    private static bool _isGameOver = false;
    public static async Task StartGame(IPlayer firstPlayer, IPlayer secondPlayer, CancellationToken token)
    {
        while (!_isGameOver)
        {
            await firstPlayer.Move();
        }
    }
    
    
}