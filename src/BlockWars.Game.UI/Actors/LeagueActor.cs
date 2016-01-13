using Akka.Actor;
using BlockWars.Game.UI.Commands;
using System.Collections.Generic;
using BlockWars.GameState.Models;
using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI.Actors
{
    public class LeagueActor : ReceiveActor
    {
        private Dictionary<string, Region> _regions = new Dictionary<string, Region>();

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

            Receive<EndLeagueCommand>(x =>
            {
                EndLeague(x);
                return true;
            });

            Receive<GetViewQuery>(x =>
            {
                Sender.Tell(GetLeagueView(x));
                return true;
            });
        }

        private LeagueViewModel GetLeagueView(GetViewQuery x)
        {
            return new LeagueViewModel
            {
                Regions = _regions.Values
            };
        }

        private void EndLeague(EndLeagueCommand x)
        {
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if(_regions.ContainsKey(x.RegionName))
            {
                var region = _regions[x.RegionName];
                region.BlockCount++;
            }
        }

        private void AddRegion(AddRegionCommand x)
        {
            if(!_regions.ContainsKey(x.Region.Name))
            {
                _regions[x.Region.Name] = x.Region;
            }
        }
    }
}