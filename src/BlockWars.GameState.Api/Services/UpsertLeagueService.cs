using System.Threading.Tasks;
using BlockWars.GameState.Models;
using AutoMapper;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.DataModels;
using System;

namespace BlockWars.GameState.Api.Services
{
    public class UpsertLeagueService : IUpsertLeague
    {
        private readonly IMappingEngine _mapper;
        private readonly ILeagueRepository _leagueRepository;

        public UpsertLeagueService(IMappingEngine mapper, ILeagueRepository leagueRepository)
        {
            _mapper = mapper;
            _leagueRepository = leagueRepository;
        }

        public Task UpsertLeagueAsync(Guid leagueId, League league)
        {
            var leagueData = _mapper.Map<LeagueData>(league);
            leagueData.LeagueId = leagueId;
            return _leagueRepository.UpsertLeagueAsync(leagueId, leagueData);
        }
    }
}
