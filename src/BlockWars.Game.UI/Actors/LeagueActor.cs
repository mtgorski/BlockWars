using Akka.Actor;
using BlockWars.Game.UI.Commands;
using System.Collections.Generic;
using BlockWars.GameState.Models;
using System;
using BlockWars.Game.UI.ViewModels;
using Akka.DI.Core;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueActor : ReceiveActor
    {
        private Dictionary<string, Region> _regions = new Dictionary<string, Region>();
        private League _league;
        private bool _expired;
        private readonly ISubscriptionRegistry _registry;

        public LeagueActor(ISubscriptionRegistry registry)
        {
            _registry = registry;

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

            Receive<SavedLeagueMessage>(async x =>
            {
                await Self.GracefulStop(TimeSpan.FromSeconds(10));
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
                NotifySubscribers();
                Context.Parent.Tell(new LeagueEndedMessage(_league.LeagueId));
                SaveLeague();
            }

            if (!_expired)
            {
                NotifySubscribers();
            }
        }

        private void NotifySubscribers()
        {
            var subscribers = _registry.GetSubscribers<LeagueViewModel>();
            foreach (var actor in subscribers)
            {
                actor.Tell(new LeagueViewModel
                {
                    League = _league,
                    Regions = _regions.Values
                });
            }
        }

        private void SaveLeague()
        {
            var view = new LeagueViewModel
            {
                League = _league,
                Regions = _regions.Values
            };

            var command = new SaveLeagueCommand(view);

            var repo = Context.ActorOf(Context.System.DI().Props<LeaguePersistenceActor>());
            repo.Tell(command);
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if (_regions.ContainsKey(x.RegionName) && !_expired)
            {
                var region = _regions[x.RegionName];
                region.BlockCount++;
            }
        }

        private void AddRegion(AddRegionCommand x)
        {
            if (!_regions.ContainsKey(x.Region.Name) && !_expired)
            {
                _regions[x.Region.Name] = x.Region;
            }
        }
    }
}