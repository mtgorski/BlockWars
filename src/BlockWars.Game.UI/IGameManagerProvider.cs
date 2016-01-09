using System.Threading.Tasks;

namespace BlockWars.Game.UI
{
    public interface IGameManagerProvider
    {
        Task<GameManager> GetFromSavedGameAsync();
        GameManager GetFromNewGame();
    }
}