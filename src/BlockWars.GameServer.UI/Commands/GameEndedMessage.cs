using BlockWars.Game.UI.ViewModels;
using System;

namespace BlockWars.Game.UI.Actors
{
    public class GameEndedMessage
    {
        public Guid GameId { get; }
        public GameViewModel FinalState { get; }

        public GameEndedMessage(Guid gameId, GameViewModel finalState)
        {
            GameId = gameId;
            FinalState = finalState;
        }
    }
}