using Akka.Actor;
using BlockWars.Game.UI.Commands;
using System.Collections.Generic;
using BlockWars.Game.UI.ViewModels;
using System.Diagnostics;
using BlockWars.Game.UI.Models;
using System.Linq;

namespace BlockWars.Game.UI.Actors
{
    public class GameActor : ReceiveActor
    {
        private Dictionary<string, RegionState> _regions = new Dictionary<string, RegionState>();
        private Models.GameState _gameState;
        private bool _expired;
        private Stopwatch _clock;

        public GameActor()
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

            Receive<InitializeGameCommand>(x =>
            {
                Initialize(x);
                return true;
            });
        }

        private void Initialize(InitializeGameCommand x)
        {
            _gameState = x.GameData;
            foreach(var region in x.Regions)
            {
                _regions[region.Name] = region;
            }
            _clock = new Stopwatch();
            _clock.Start();
        }

        private void CheckState(CheckStateCommand x)
        {
            if (ShouldBeExpired())
            {
                _clock.Stop();
                _expired = true;
                var endingMessage = new GameEndedMessage(_gameState.GameId, GetCurrentView());
                Context.System.EventStream.Publish(endingMessage);
            }
            else
            {
                PublishCurrentState();
            }
             
        }

        private bool ShouldBeExpired()
        {
            return _clock.ElapsedMilliseconds > _gameState.Duration && !_expired;
        }

        private void PublishCurrentState()
        {
            var currentState = GetCurrentView();
            Context.System.EventStream.Publish(currentState);
        }

        private GameViewModel GetCurrentView()
        {
            return new GameViewModel
            (
                _gameState.Duration - _clock.ElapsedMilliseconds,
                _gameState,
                _regions.Values.ToList()
            );
        }

        private void BuildBlock(BuildBlockCommand x)
        {
            if (_regions.ContainsKey(x.RegionName) && !_expired)
            {
                var region = _regions[x.RegionName];
                _regions[x.RegionName] = region.AddBlocks(1);
                Sender.Tell(new BlockBuiltMessage(x.ConnectionId, _gameState.GameId));
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