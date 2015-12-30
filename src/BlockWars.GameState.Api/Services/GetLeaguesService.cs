using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using AutoMapper;
using BlockWars.GameState.Api.Repositories;

namespace BlockWars.GameState.Api.Services
{
    public class GetLeaguesService : IGetLeagues
    {
        private readonly IMappingEngine _mapper;
        private readonly ILeagueRepository _leagueRepository; 

        public GetLeaguesService(IMappingEngine mapper, ILeagueRepository leagueRepository)
        {
            _mapper = mapper;
            _leagueRepository = leagueRepository;
        }

        public async Task<ICollection<League>> GetLeaguesAsync()
        {
            var leagueData = await _leagueRepository.GetLeaguesAsync();

            var leagues = _mapper.Map<List<League>>(leagueData);

            return leagues;
        }
    }
}
