using BlockWars.GameState.Models;

namespace BlockWars.Game.UI.Commands
{
    public class InitializeLeagueCommand
    {
        public League LeagueData { get;  }

        public InitializeLeagueCommand(League league)
        {
            LeagueData = league;
        }
    }
}
