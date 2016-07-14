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
        private readonly Dictionary<string, RegionState> _regions = new Dictionary<string, RegionState>();
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

        private void Initialize(InitializeGameCommand command)
        {
            _gameState = command.GameData;
            foreach(var region in command.Regions)
            {
                _regions[region.Name] = region;
            }
            _clock = new Stopwatch();
            _clock.Start();
        }

        private void CheckState(CheckStateCommand command)
        {
            if (!_expired && ShouldBeExpired())
            {
                _clock.Stop();
                _expired = true;
                var endingMessage = new GameEndedMessage(_gameState.GameId, GetCurrentView());
                Context.System.EventStream.Publish(endingMessage);
            }
            else if(!_expired)
            {
                PublishCurrentState();
            }
             
        }

        private bool ShouldBeExpired()
        {
            return _clock.ElapsedMilliseconds > _gameState.Duration;
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

        private void BuildBlock(BuildBlockCommand command)
        {
            if (_regions.ContainsKey(command.RegionName) && !_expired)
            {
                var region = _regions[command.RegionName];
                _regions[command.RegionName] = region.AddBlocks(1);
                Sender.Tell(new BlockBuiltMessage(command.ConnectionId, _gameState.GameId));
            }
        }

        private void AddRegion(AddRegionCommand command)
        {
            if (!_regions.ContainsKey(command.Region.Name) && !_expired)
            {
                _regions[command.Region.Name] = command.Region;
            }
        }
    }
}