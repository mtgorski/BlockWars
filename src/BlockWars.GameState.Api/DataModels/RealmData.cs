using MongoDB.Bson.Serialization.Attributes;

namespace BlockWars.GameState.Api.DataModels
{
    [BsonIgnoreExtraElements]
    public class RealmData
    {
        public string RealmId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
