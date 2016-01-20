using System;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueEndedMessage
    {
        public Guid LeagueId { get; }

        public LeagueEndedMessage(Guid leagueId)
        {
            LeagueId = leagueId;
        }
    }
}