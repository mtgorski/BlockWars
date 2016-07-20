using System;
using Akka.Actor;
using BlockWars.Game.UI.ViewModels;
using System.Linq;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Threading.Tasks;
using BlockWars.Game.UI.Queries;

namespace BlockWars.Game.UI.Actors
{
    public class LeaderboardActor : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;

        public LeaderboardActor(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            Receive<GameViewModel>(async x =>
            {
                await OnGameViewReceivedAsync(x);
            });

            Receive<GameEndedMessage>(async x =>
            {
                await OnGameEndedAsync(x);
            });
        }

        private async Task OnGameEndedAsync(GameEndedMessage message)
        {
            var view = message.FinalState;
            var leaders = await GetLeadersAsync(view);
            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.All.updateLeaderboard(new { GameId = view.Game.GameId, Final = true, Leaders = leaders });
        }

        private async Task<Leader[]> GetLeadersAsync(GameViewModel view)
        {
            var leaderTasks = view.Players
                .OrderByDescending(x => x.BlockCount)
                .Take(10)
                .Select(async x => {

                    return new Leader { BlockCount = x.BlockCount, Name = await GetName(x) };
                })
                .ToList();
            return await Task.WhenAll(leaderTasks);
        }

        private async Task OnGameViewReceivedAsync(GameViewModel view)
        {
            var leaders = await GetLeadersAsync(view);

            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.All.updateLeaderboard(new { GameId = view.Game.GameId, Final = false, Leaders = leaders });
            Context.Sender.Tell(new FinishedMessage());
        }

        class Leader
        {
            public int BlockCount;
            public string Name;
        }

        private async Task<string> GetName(PlayerBlockCount x)
        {
            try
            {
                var name = await Context.ActorSelection("/user/playerSupervisor").Ask<string>(new UserNameQuery(x.ConnectionId), TimeSpan.FromMilliseconds(20));
                return name ?? "(unknown)";
            }
            catch(TaskCanceledException)
            {
                return "(unknown)";
            }

        }
    }
}
