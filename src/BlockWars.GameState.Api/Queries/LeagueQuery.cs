using BlockWars.GameState.Api.DataModels;
using System;
using MongoDB.Driver;

namespace BlockWars.GameState.Api.Queries
{
    public class LeagueQuery : IQuery<LeagueData>
    {
        public bool IsCurrent { get; set; }

        public FilterDefinition<LeagueData> ToFilterDefinition()
        {
            var filterBuilder = Builders<LeagueData>.Filter;

            if(IsCurrent)
                return filterBuilder.Gt(x => x.ExpiresAt, DateTime.UtcNow);

            return filterBuilder.Lt(x => x.ExpiresAt, DateTime.UtcNow);
        }
    }
}
