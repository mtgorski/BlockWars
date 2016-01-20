using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI.Actors
{
    public class SaveLeagueCommand
    {
        public LeagueViewModel ViewModel { get; }

        public SaveLeagueCommand(LeagueViewModel view)
        {
            ViewModel = view;
        }
    }
}