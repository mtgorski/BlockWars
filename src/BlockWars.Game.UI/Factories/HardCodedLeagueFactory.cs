using BlockWars.Game.UI.Models;
using BlockWars.Game.UI.Options;
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

        public LeagueState GetLeague()
        {
            var now = DateTime.UtcNow;
            return new LeagueState(
                Guid.NewGuid(),
                now.ToString(),
                "Automatically generated league",
                now,
                120000L
            );
        }
    }
}
