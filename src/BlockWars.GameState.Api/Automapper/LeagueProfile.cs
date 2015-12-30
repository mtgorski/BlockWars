using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Automapper
{
    public class LeagueProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();

            CreateMap<League, LeagueData>();
            CreateMap<LeagueData, League>();
        }
    }
}
