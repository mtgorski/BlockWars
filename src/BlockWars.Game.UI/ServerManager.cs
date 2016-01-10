using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using System.Threading.Tasks;

namespace BlockWars.Game.UI
{
    public class ServerManager : IServerManager
    {
        private readonly object _gameLoopLock = new object();
        private Task _gameLoop;
        private IGameManagerProvider _gameManagerProvider;

        private IHubCallerConnectionContext<dynamic> _clients;

        public CancellationTokenSource GameLoopCancellationSource { get; private set; }

        public GameManager CurrentGameManager
        {
            get; private set;
        }

        public ServerManager(IGameManagerProvider managerProvider)
        {
            GameLoopCancellationSource = new CancellationTokenSource();
            _gameManagerProvider = managerProvider;
        }

        public void EnsureGameLoop(IHubCallerConnectionContext<dynamic> clients)
        {
            lock(_gameLoopLock)
            {
                if(_gameLoop == null)
                {
                    _clients = clients;
                    _gameLoop = GameLoop(GameLoopCancellationSource.Token);
                }
            }
        }

        private async Task GameLoop(CancellationToken token)
        {
            await Task.Yield();

            CurrentGameManager = await _gameManagerProvider.GetFromSavedGameAsync();
            if(CurrentGameManager == null)
            {
                CurrentGameManager = _gameManagerProvider.GetFromNewGame();
                await CurrentGameManager.SaveGameAsync();
            }

            while(!token.IsCancellationRequested)
            {
                var viewModel = CurrentGameManager.GetCurrentLeague();
                if (viewModel != null)
                {
                    if (_clients != null)
                    {
                        _clients.All.updateRegionInfo(viewModel);
                    }
                }
                else
                {
                    await CurrentGameManager.SaveGameAsync();
                    CurrentGameManager = _gameManagerProvider.GetFromNewGame();
                    await CurrentGameManager.SaveGameAsync();
                }

                await Task.Delay(15);
            }

        }
    }
}
