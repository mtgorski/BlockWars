using System;

namespace BlockWars.Game.UI.Actors
{
    public class BlockBuiltMessage
    {
        public BlockBuiltMessage(string connectionId, Guid gameId)
        {
            ConnectionId = connectionId;
            GameId = gameId;
        }

        public string ConnectionId { get; }
        public Guid GameId { get; }
    }
}