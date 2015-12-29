using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockWars.GameState.Models;
using AutoMapper;
using BlockWars.GameState.Api.Repositories;

namespace BlockWars.GameState.Api.Services
{
    public class GetRealmsService : IGetRealms
    {
        private readonly IMappingEngine _mapper;
        private readonly IRealmRepository _realmRepository; 

        public GetRealmsService(IMappingEngine mapper, IRealmRepository realmRepository)
        {
            _mapper = mapper;
            _realmRepository = realmRepository;
        }

        public async Task<ICollection<Realm>> GetRealmsAsync()
        {
            var realmData = await _realmRepository.GetRealmsAsync();

            var realms = _mapper.Map<List<Realm>>(realmData);

            return realms;
        }
    }
}
