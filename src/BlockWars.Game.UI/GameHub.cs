using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        private readonly ServerManager _serverManager = (ServerManager)Startup.ServiceProvider.GetService(typeof(ServerManager));

        public override Task OnConnected()
        {
            _serverManager.EnsureGameLoop(Clients);
            return Task.FromResult(0);
        }

        public void BuildBlock(string regionName)
        {
            _serverManager.CurrentGameManager.BuildBlock(regionName);
        }

    }
}
