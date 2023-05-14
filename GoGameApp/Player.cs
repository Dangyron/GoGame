using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GoGame.Utility;

namespace GoGameApp;

public class Player : IPlayer
{
    private readonly Canvas _boardCanvas;
    public StonesStates StoneStates { get; }
    public string Name { get; }

    public bool Resign { get; private set; } = false;
    private Stone _stone;
    public Player(StonesStates stoneStates, string name, Canvas boardCanvas)
    {
        _boardCanvas = boardCanvas;
        StoneStates = stoneStates;
        Name = name;
        _stone = new Stone(StoneStates);
    }

    public async Task Move()
    {
        await Task.Run((() =>
        {
                
        }));
    }
}