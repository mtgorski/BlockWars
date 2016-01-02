using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BlockWars.GameState.Api.DataModels
{
    [BsonIgnoreExtraElements]
    public class LeagueData
    {
        public Guid LeagueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
