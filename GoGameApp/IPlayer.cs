using System.Threading.Tasks;

namespace GoGameApp;

public interface IPlayer
{
    public StonesColour StoneColour { get; }
    public string Name { get; }
    public bool Resign { get; }
    Task Move();
}