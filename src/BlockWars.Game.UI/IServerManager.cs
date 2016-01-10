using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        void BuildBlock(Guid leagueId, string regionName);

        void AddRegion(Guid leagueId, Region regionName);

        LeagueViewModel GetCurrentLeagueView();

        CancellationTokenSource GameLoopCancellationSource { get; }

        void EnsureGameLoop(IHubCallerConnectionContext<dynamic> clients);
    }
}