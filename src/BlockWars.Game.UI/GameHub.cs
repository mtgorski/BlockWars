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

        public void BuildBlock(string leagueIdInput, string regionName)
        {
            Guid leagueId;
            if(Guid.TryParse(leagueIdInput, out leagueId))
            {
                _serverManager.BuildBlock(leagueId, regionName);
            }
        }

    }
}
