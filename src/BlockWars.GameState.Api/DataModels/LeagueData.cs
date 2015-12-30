using MongoDB.Bson.Serialization.Attributes;

namespace BlockWars.GameState.Api.DataModels
{
    [BsonIgnoreExtraElements]
    public class LeagueData
    {
        public string LeagueId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
