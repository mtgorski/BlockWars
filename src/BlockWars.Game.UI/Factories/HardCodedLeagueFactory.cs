using BlockWars.Game.UI.Options;
using BlockWars.GameState.Models;
using Microsoft.Extensions.OptionsModel;
using System;

namespace BlockWars.Game.UI.Strategies
{
    public class HardCodedLeagueFactory : INewLeagueFactory
    {
        private readonly IOptions<GameDuration> _options;

        public HardCodedLeagueFactory(IOptions<GameDuration> options)
        {
            _options = options;
        }

        public League GetLeague()
        {
            var league = new League
            {
                LeagueId = Guid.NewGuid(),
                Name = DateTime.UtcNow.ToString(),
                Description = "Automatically generated league",
                Duration = 120000L
            };

            return league;
        }
    }
}
