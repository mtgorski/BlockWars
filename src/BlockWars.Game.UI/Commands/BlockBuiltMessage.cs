using System;

namespace BlockWars.Game.UI.Actors
{
    public class BlockBuiltMessage
    {
        public BlockBuiltMessage(string connectionId, Guid leagueId)
        {
            ConnectionId = connectionId;
            LeagueId = leagueId;
        }

        public string ConnectionId { get; internal set; }
        public Guid LeagueId { get; internal set; }
    }
}