using System;
using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;

namespace BlockWars.Game.UI
{
    public interface IGameState
    {
        bool IsTheCurrentGame { get; }
        LeagueViewModel ToView();
        void AddRegion(Region region);
        void BuildBlock(string regionName);
    }

    public class GameState : IGameState
    {
        private readonly League _league;

        public bool IsTheCurrentGame
        {
            get
            {
                return _league.ExpiresAt > DateTime.UtcNow;
            }
        }

        public ConcurrentDictionary<string, Region> Regions { get; private set; }

        public class InvalidRegionException : Exception
        {
            public InvalidRegionException(string message) : base(message)
            {
            }
        }

        public GameState(League league)
        {
            Regions = new ConcurrentDictionary<string, Region>();
            _league = league;
        }

        public void AddRegion(Region region)
        {
            if(!Regions.TryAdd(region.Name, region))
            {
                throw new InvalidRegionException(string.Format("A region with name {0} already exists", region.Name));
            }
        }

        public void BuildBlock(string regionName)
        {
            Region regionToUpdate;
            if (Regions.TryGetValue(regionName, out regionToUpdate))
            {
                lock (regionToUpdate)
                {
                    regionToUpdate.BlockCount++;
                }
            }
        }

        public LeagueViewModel ToView()
        {
            var regions = new List<Region>();
            //TODO: make sure this is threadsafe
            using (var enumerator = Regions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    regions.Add(enumerator.Current.Value);
                }
            }
            //TODO: expose copies instead of the underlying values    
            var view = new LeagueViewModel
            {
                League = _league,
                Regions = regions
            };

            return view;
        }
    }
}
