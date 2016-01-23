using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI
{
    public interface INewInstanceFactory
    {
        LeagueViewModel GetInstance();
    }
}