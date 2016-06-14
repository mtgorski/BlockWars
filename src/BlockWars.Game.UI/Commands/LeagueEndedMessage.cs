using BlockWars.Game.UI.ViewModels;
using System;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueEndedMessage
    {
        public Guid LeagueId { get; }
        public LeagueViewModel FinalState { get; }

        public LeagueEndedMessage(Guid leagueId, LeagueViewModel finalState)
        {
            LeagueId = leagueId;
            FinalState = finalState;
        }
    }
}