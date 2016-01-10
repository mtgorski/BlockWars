using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private readonly IServerManager _serverManager;

        public GameHub(IServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        public override Task OnConnected()
        {
            _serverManager.EnsureGameLoop(Clients);
            return Task.FromResult(0);
        }

        public void BuildBlock(string leagueIdInput, string regionName)
        {
            Guid leagueId;
            if(Guid.TryParse(leagueIdInput, out leagueId))
            {
                _serverManager.CurrentGameManager.BuildBlock(leagueId, regionName);
            }
        }

    }
}
