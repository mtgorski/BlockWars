using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
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

        public void BuildBlock(string gameIdInput, string regionName)
        {
            Guid gameId;
            if(Guid.TryParse(gameIdInput, out gameId))
            {
                _serverManager.BuildBlock(gameId, regionName, Context.ConnectionId);
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

        public void ChangeName(string name)
        {
            _serverManager.ChangeName(Context.ConnectionId, name);
        }

    }
}
