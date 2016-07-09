using System;

namespace BlockWars.Game.UI.Models
{
    public struct LeagueState
    {
        public Guid LeagueId { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime CreatedAt { get; }

        public long Duration { get; }

        public LeagueState(Guid leagueId, string name, string description, DateTime createdAt, long duration)
        {
            LeagueId = leagueId;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Duration = duration;
        }
    }
}
