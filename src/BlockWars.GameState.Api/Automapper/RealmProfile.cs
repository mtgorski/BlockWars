using AutoMapper;
using BlockWars.GameState.Api.DataModels;
using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockWars.GameState.Api.Automapper
{
    public class RealmProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();

            CreateMap<Realm, RealmData>();
            CreateMap<RealmData, Realm>();
        }
    }
}
