using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using BlockWars.GameState.Api.Repositories;
using AutoMapper;

namespace BlockWars.GameState.Api.Services
{
    public class GetRegionsService : IGetRegions
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMappingEngine _mapper;

        public GetRegionsService(IRegionRepository regionRepository, IMappingEngine mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<Region>> GetRegionsAsync(Guid leagueId)
        {
            var regionData = await _regionRepository.GetRegionsAsync(leagueId);
            var regions = _mapper.Map<List<Region>>(regionData);

            return regions;
        }
    }
}
