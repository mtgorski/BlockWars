using BlockWars.GameState.Models;
using System;

namespace BlockWars.Game.UI.Strategies
{
    public class HardCodedLeagueFactory : INewLeagueFactory
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
}
