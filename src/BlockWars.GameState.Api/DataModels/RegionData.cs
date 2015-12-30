using MongoDB.Bson.Serialization.Attributes;

namespace BlockWars.GameState.Api.DataModels
{
    [BsonIgnoreExtraElements]
    public class RegionData
    {
        public string RegionId { get; set; }

        public string RealmId { get; set; }

        public string Name { get; set; }

        [BsonIgnore]
        public long RedCount { get; set; }

        [BsonIgnore]
        public long BlueCount { get; set; }
    }
}
