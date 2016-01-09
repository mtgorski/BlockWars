using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BlockWars.Game.UI
{
    [HubName("game")]
    public class GameHub : Hub
    {
        public override Task OnConnected()
        {
            ServerManager.Instance.EnsureGameLoop(Clients);
            return Task.FromResult(0);
        }

        public void BuildBlock(string regionName)
        {
            ServerManager.Instance.CurrentGameManager.BuildBlock(regionName);
        }

    }
}
