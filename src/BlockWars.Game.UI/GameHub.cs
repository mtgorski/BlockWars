using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

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

        public void BuildBlock(string leagueIdInput, string regionName)
        {
            Guid leagueId;
            if(Guid.TryParse(leagueIdInput, out leagueId))
            {
                _serverManager.BuildBlock(leagueId, regionName, Context.ConnectionId);
            }
        }

        public override Task OnConnected()
        {
            _serverManager.AddConnectedUser(Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _serverManager.RemoveConnectedUser(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

    }
}
