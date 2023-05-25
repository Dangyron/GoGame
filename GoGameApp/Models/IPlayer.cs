using System.Threading.Tasks;
using System.Windows;
using GoGame.Utility;

namespace GoGameApp.Models;

public interface IPlayer
{
    public StonesStates StoneStates { get; }
    public string Name { get; }
    public bool Resign { get; }
    Task Move(Point point);
}