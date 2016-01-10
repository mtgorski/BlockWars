using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;

namespace BlockWars.Game.UI
{
    public interface IServerManager
    {
        GameManager CurrentGameManager { get; }
        CancellationTokenSource GameLoopCancellationSource { get; }

        void EnsureGameLoop(IHubCallerConnectionContext<dynamic> clients);
    }
}