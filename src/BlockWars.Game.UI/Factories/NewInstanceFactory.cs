using BlockWars.Game.UI.ViewModels;
using BlockWars.Game.UI.Strategies;

namespace BlockWars.Game.UI
{
    public class NewInstanceFactory : INewInstanceFactory
    {
        private readonly INewLeagueFactory _newLeagueStrategy;
        private readonly INewRegionsFactory _newRegionsStrategy;

        public NewInstanceFactory(INewLeagueFactory newLeagueStrategy, INewRegionsFactory newRegionsStrategy)
        {
            _newLeagueStrategy = newLeagueStrategy;
            _newRegionsStrategy = newRegionsStrategy;
        }

        public INewRegionsFactory NewRegionsStrategy
        {
            get
            {
                return _newRegionsStrategy;
            }
        }

        public LeagueViewModel GetInstance()
        {
            var league = _newLeagueStrategy.GetLeague();
            var regions = NewRegionsStrategy.GetRegions();

            return new LeagueViewModel
            {
                League = league,
                Regions = regions
            };
        }
    }
    
}
