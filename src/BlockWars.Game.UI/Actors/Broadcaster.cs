using Akka.Actor;
using BlockWars.Game.UI.Actors;
using BlockWars.Game.UI.ViewModels;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Linq;

namespace BlockWars.Game.UI
{
    public class Broadcaster : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;

        public Broadcaster(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            Receive<GameViewModel>(x =>
            {
                BroadcastState(x);
                return true;
            });

            Receive<GameEndedMessage>(x =>
            {
                BroadcastEnd(x);
                return true;
            });
        }

        private void BroadcastEnd(GameEndedMessage endMessage)
        {
            var hub = _connectionManager.GetHubContext<GameHub>();
            var orderedRegions = endMessage.FinalState.Regions.OrderByDescending(x => x.BlockCount);
            var topScore = orderedRegions.First().BlockCount;
            var winners = orderedRegions.Where(x => x.BlockCount == topScore).ToList();

            var message = "";
            if (winners.Count == 1)
            {
                message = $"The {winners.First().Name} region has won!";
            }
            else
            {
                var tiedRegions = string.Join(" and ", winners.Select(x => x.Name));
                message = $"It's a tie between {tiedRegions}!";
            }

            hub.Clients.All.onGameEnd(message);
        }

        private void BroadcastState(GameViewModel currentGame)
        {
            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.All.updateRegionInfo(currentGame);
        }
    }
}