using System.Threading.Tasks;
using GoGame.Utility;

namespace GoGameApp;

public interface IPlayer
{
    public StonesStates StoneStates { get; }
    public string Name { get; }
    public bool Resign { get; }
    Task Move();
}