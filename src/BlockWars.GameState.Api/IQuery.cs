using BlockWars.GameState.Api.DataModels;
using MongoDB.Driver;

namespace BlockWars.GameState.Api
{
    public interface IQuery<T>
    {
        FilterDefinition<LeagueData> ToFilterDefinition();
    }
}
