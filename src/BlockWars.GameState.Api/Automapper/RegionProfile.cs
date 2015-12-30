using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Models;

namespace BlockWars.GameState.Api.Automapper
{
    public class RegionProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Region, RegionData>()
               .ForMember(dest => dest.LeagueId, o => o.Ignore());
            CreateMap<RegionData, Region>();
        }
    }
}
