using Akka.Actor;
using BlockWars.Game.UI.Commands;
using System.Collections.Generic;
using BlockWars.Game.UI.ViewModels;
using System.Diagnostics;
using BlockWars.Game.UI.Models;
using System.Linq;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueActor : ReceiveActor
    {
        private Dictionary<string, RegionState> _regions = new Dictionary<string, RegionState>();
        private LeagueState _league;
        private bool _expired;
        private Stopwatch _clock;

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
            _clock = new Stopwatch();
            _clock.Start();
        }

        private void CheckState(CheckStateCommand x)
        {
            if (_clock.ElapsedMilliseconds > _league.Duration && !_expired)
            {
                _clock.Stop();
                _expired = true;
                NotifySubscribers();
                Context.System.EventStream.Publish(new LeagueEndedMessage(_league.LeagueId, GetCurrentView()));
            }

            if (!_expired)
            {
                NotifySubscribers();
            }
        }

        private void NotifySubscribers()
        {
            LeagueViewModel currentState = GetCurrentView();
            Context.System.EventStream.Publish(currentState);
        }

        private LeagueViewModel GetCurrentView()
        {
            return new LeagueViewModel
            (
                _league.Duration - _clock.ElapsedMilliseconds,
                _league,
                _regions.Values.ToList()
            );
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if (_regions.ContainsKey(x.RegionName) && !_expired)
            {
                var region = _regions[x.RegionName];
                _regions[x.RegionName] = region.AddBlocks(1);
                Context.ActorSelection("akka://BlockWars/user/stats" + x.ConnectionId).Tell(new BlockBuiltMessage { ConnectionId = x.ConnectionId, LeagueId = _league.LeagueId });
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