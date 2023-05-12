using System.Windows.Media;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp;

public class Stone
{
    public Brush StoneColour { get; }

    public Stone(StonesColour stoneColour)
    {
        StoneColour = stoneColour switch
        {
            StonesColour.White => Constants.WhiteStone,
            _ => Constants.BlackStone
        };

    }

    public Ellipse GetStone()
    {
        return new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = StoneColour
        };
    }
}