

using BlockWars.GameState.Models;
using System.Collections.Generic;
using System;
using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI
{
    public class NewInstanceFactory : INewInstanceFactory
    {
        private readonly INewLeagueStrategy _newLeagueStrategy;
        private readonly INewRegionsStrategy _newRegionsStrategy;

        public NewInstanceFactory(INewLeagueStrategy newLeagueStrategy, INewRegionsStrategy newRegionsStrategy)
        {
            _newLeagueStrategy = newLeagueStrategy;
            _newRegionsStrategy = newRegionsStrategy;
        }

        public LeagueViewModel GetInstance()
        {
            var league = _newLeagueStrategy.GetLeague();
            var regions = _newRegionsStrategy.GetRegions();

            return new LeagueViewModel
            {
                League = league,
                Regions = regions
            };
        }
    }

    public interface INewRegionsStrategy
    {
        ICollection<Region> GetRegions();
    }

    public interface INewLeagueStrategy
    {
        League GetLeague();
    }

    public class HardCodedLeagueStrategy : INewLeagueStrategy
    {
        public League GetLeague()
        {
            var nextExpiryTime = DateTime.UtcNow.AddMinutes(1);
            var roundedExpiryTime = new DateTime(nextExpiryTime.Year, nextExpiryTime.Month, nextExpiryTime.Day, nextExpiryTime.Hour, nextExpiryTime.Minute, 0, DateTimeKind.Utc);
            var league = new League
            {
                LeagueId = Guid.NewGuid(),
                Name = DateTime.UtcNow.ToString(),
                Description = "Automatically generated league",
                ExpiresAt = roundedExpiryTime
            };

            return league;
        }
    }

    public class HardCodedRegionsStrategy : INewRegionsStrategy
    {
        public ICollection<Region> GetRegions()
        {
            return new List<Region>
            {
                new Region
                {
                    Name = "Cats",
                    RegionId = Guid.NewGuid()
                },
                new Region
                {
                    Name = "Dogs",
                    RegionId = Guid.NewGuid()
                }
            };
        }
    }
}
