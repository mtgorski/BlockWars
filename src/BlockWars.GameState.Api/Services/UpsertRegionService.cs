using System.Threading.Tasks;
using BlockWars.GameState.Models;
using BlockWars.GameState.Api.Repositories;
using AutoMapper;
using BlockWars.GameState.Api.DataModels;

namespace BlockWars.GameState.Api.Services
{
    public class UpsertRegionService : IUpsertRegion
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMappingEngine _mapper;

        public UpsertRegionService(IMappingEngine mapper, IRegionRepository regionRepository)
        {
            _mapper = mapper;
            _regionRepository = regionRepository;
        }

        public Task UpsertRegionAsync(string realmId, string regionId, Region region)
        {
            var regionData = _mapper.Map<RegionData>(region);
            regionData.RealmId = realmId;
            regionData.RegionId = regionId;

            return _regionRepository.UpsertRegionAsync(regionId, regionData);
        }
    }
}
