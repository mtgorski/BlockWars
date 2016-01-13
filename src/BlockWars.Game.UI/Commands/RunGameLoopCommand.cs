using Microsoft.AspNet.SignalR.Hubs;

namespace BlockWars.Game.UI
{
    public class RunGameLoopCommand
    {
        public IHubCallerConnectionContext<dynamic> Clients { get; private set; }

        public RunGameLoopCommand(IHubCallerConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

    }
}