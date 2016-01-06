using System;
using BlockWars.Game.UI.ViewModels;
using BlockWars.GameState.Models;

namespace BlockWars.Game.UI
{
    public interface IGameState
    {
        bool IsTheCurrentGame { get; }
        LeagueViewModel ToView();
        void AddRegion(Region region);
        void BuildBlock(Guid regionId);
    }

    public class GameState : IGameState
    {
        public bool IsTheCurrentGame { get; set; }

        public void AddRegion(Region region)
        {
            throw new NotImplementedException();
        }

        public void BuildBlock(Guid regionId)
        {
            throw new NotImplementedException();
        }

        public virtual LeagueViewModel ToView()
        {
            throw new NotImplementedException();
        }
    }
}
