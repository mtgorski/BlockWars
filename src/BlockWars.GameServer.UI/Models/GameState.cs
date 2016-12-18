using System;

namespace BlockWars.Game.UI.Models
{
    public struct GameState
    {
        public Guid GameId { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime CreatedAt { get; }

        public long Duration { get; }

        public GameState(Guid gameId, string name, string description, DateTime createdAt, long duration)
        {
            GameId = gameId;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Duration = duration;
        }
    }
}
