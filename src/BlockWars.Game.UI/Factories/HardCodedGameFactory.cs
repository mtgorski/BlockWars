using BlockWars.Game.UI.Models;
using BlockWars.Game.UI.Options;
using Microsoft.Extensions.OptionsModel;
using System;

namespace BlockWars.Game.UI.Strategies
{
    public class HardCodedGameFactory : INewGameFactory
    {
        private readonly IOptions<GameDuration> _options;

        public HardCodedGameFactory(IOptions<GameDuration> options)
        {
            _options = options;
        }

        public Models.GameState GetGameState()
        {
            var now = DateTime.UtcNow;
            return new Models.GameState(
                Guid.NewGuid(),
                now.ToString(),
                "Automatically generated game",
                now,
                120000L
            );
        }
    }
}
