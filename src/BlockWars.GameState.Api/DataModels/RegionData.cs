using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BlockWars.GameState.Api.DataModels
{
    [BsonIgnoreExtraElements]
    public class RegionData
    {
        public Guid RegionId { get; set; }

        public Guid LeagueId { get; set; }

        public string Name { get; set; }

        public long BlockCount { get; set; }
    }
}
