using BlockWars.Game.UI.Models;

namespace BlockWars.Game.UI.Commands
{
    public class InitializeLeagueCommand
    {
        public LeagueState LeagueData { get;  }

        public InitializeLeagueCommand(LeagueState league)
        {
            LeagueData = league;
        }
    }
}
