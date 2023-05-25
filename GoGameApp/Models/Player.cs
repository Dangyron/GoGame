using System.Threading.Tasks;
using System.Windows;
using GoGame.Utility;

namespace GoGameApp.Models;

public class Player : IPlayer
{
    private readonly Board _board;
    public StonesStates StoneStates { get; }
    public string Name { get; }
    
    public bool Resign { get; private set; } = false;
    private Stone _stone;
    public Player(StonesStates stoneStates, string name, Board board)
    {
        StoneStates = stoneStates;
        Name = name;
        _board = board;
        _stone = new Stone(StoneStates);
    }

    public async Task Move(Point point)
    {
        
    }
}