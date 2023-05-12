using System.Windows.Media;
using System.Windows.Shapes;
using GoGame.Utility;

namespace GoGameApp;

public class Stone
{
    public Brush StoneColour { get; }

    private Ellipse _ellipse;
    public Stone(StonesColour stoneColour)
    {
        StoneColour = stoneColour switch
        {
            StonesColour.White => Constants.WhiteStone,
            _ => Constants.BlackStone
        };
        _ellipse = new Ellipse
        {
            Width = Constants.StoneSize,
            Height = Constants.StoneSize,
            Fill = StoneColour
        };
    }

    public Ellipse GetStone()
    {
        return _ellipse;
    }
}