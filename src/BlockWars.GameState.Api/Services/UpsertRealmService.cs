using System.Threading.Tasks;
using BlockWars.GameState.Models;
using AutoMapper;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.DataModels;

namespace BlockWars.GameState.Api.Services
{
    public class UpsertRealmService : IUpsertRealm
    {
        private readonly IMappingEngine _mapper;
        private readonly IRealmRepository _realmRepository;

        public UpsertRealmService(IMappingEngine mapper, IRealmRepository realmRepository)
        {
            _mapper = mapper;
            _realmRepository = realmRepository;
        }

        public Task UpsertRealmAsync(string realmId, Realm realm)
        {
            var realmData = _mapper.Map<RealmData>(realm);
            realmData.RealmId = realmId;
            return _realmRepository.UpsertRealmAsync(realmId, realmData);
        }
    }
}
