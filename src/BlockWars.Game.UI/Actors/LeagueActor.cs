﻿using Akka.Actor;
using BlockWars.Game.UI.Commands;
using System.Collections.Generic;
using BlockWars.GameState.Models;
using System;
using BlockWars.Game.UI.ViewModels;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueActor : ReceiveActor
    {
        private Dictionary<string, Region> _regions = new Dictionary<string, Region>();
        private League _league;
        private bool _expired;

        public LeagueActor()
        {
            Receive<AddRegionCommand>(x =>
            {
                AddRegion(x);
                return true;
            });

            Receive<BuildBlockCommand>(x =>
            {
                BuildBlock(x);
                return true;
            });

            Receive<CheckStateCommand>(x =>
            {
                CheckState(x);
                return true;
            });

            Receive<InitializeLeagueCommand>(x =>
            {
                Initialize(x);
                return true;
            });
        }

        private void Initialize(InitializeLeagueCommand x)
        {
            _league = x.LeagueData;
        }

        private void CheckState(CheckStateCommand x)
        {
            if (DateTime.UtcNow > _league.ExpiresAt && !_expired)
            {
                _expired = true;
                Sender.Tell(new LeagueEndedMessage());
                TellStateToBroadcaster();
            }
            
            if(!_expired)
            {
                TellStateToBroadcaster();
            }
        }

        private void TellStateToBroadcaster()
        {
            var view = new LeagueViewModel
            {
                League = _league,
                Regions = _regions.Values
            };
            Context.ActorSelection("/user/broadcaster").Tell(view);
            Debug.WriteLine(JsonConvert.SerializeObject(view));
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if(_regions.ContainsKey(x.RegionName) && !_expired)
            {
                var region = _regions[x.RegionName];
                region.BlockCount++;
            }
        }

        private void AddRegion(AddRegionCommand x)
        {
            if(!_regions.ContainsKey(x.Region.Name) && !_expired)
            {
                _regions[x.Region.Name] = x.Region;
            }
        }
    }
}