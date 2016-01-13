using Microsoft.AspNet.SignalR.Hubs;

namespace BlockWars.Game.UI
{
    public class EnsureGameLoopCommand
    {
        public IHubCallerConnectionContext<dynamic> Clients { get; private set; }

        public EnsureGameLoopCommand(IHubCallerConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }
    }
}