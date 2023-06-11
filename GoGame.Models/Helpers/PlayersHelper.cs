using GoGame.Models.Models;
using GoGame.Utility;

namespace GoGame.Models.Helpers;

public static class PlayersHelper
{
    public static StonesStates GetEnemyColour(this IPlayer[] players, StonesStates stonesStates)
    {
        return players.Single(i => i.StoneColour != stonesStates).StoneColour;
    }
}