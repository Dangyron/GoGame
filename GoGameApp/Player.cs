using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoGameApp;

public class Player : IPlayer
{
    private readonly Canvas _boardCanvas;
    public StonesColour StoneColour { get; }
    public string Name { get; }

    public bool Resign { get; private set; } = false;
    private Stone _stone;
    public Player(StonesColour stoneColour, string name, Canvas boardCanvas)
    {
        _boardCanvas = boardCanvas;
        StoneColour = stoneColour;
        Name = name;
        _stone = new Stone(StoneColour);
    }

    public async Task Move()
    {
        await Task.Run((() =>
        {
                
        }));
    }
}