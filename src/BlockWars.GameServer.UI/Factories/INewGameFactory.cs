using BlockWars.Game.UI.Models;

namespace BlockWars.Game.UI.Strategies
{
    public interface INewGameFactory
    {
        Models.GameState GetGameState();
    }
}
