using System;
using Akka.Actor;
using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI.Actors
{
    public class LeaderboardBuffer : ReceiveActor
    {
        private bool _processingMessage;

        public LeaderboardBuffer()
        {
            Receive<GameViewModel>(x =>
            {
                OnGameViewReceive(x);
                return true;
            });

            Receive<GameEndedMessage>(x =>
            {
                OnGameEnded(x);
                return true;
            });

            Receive<FinishedMessage>(x =>
            {
                OnFinishedMessage(x);
                return true;
            });
        }

        private void OnGameEnded(GameEndedMessage message)
        {
            Context.ActorSelection("/user/leaderboard").Tell(message);
        }

        private void OnFinishedMessage(FinishedMessage message)
        {
            _processingMessage = false;
        }

        private void OnGameViewReceive(GameViewModel gameView)
        {
            if(!_processingMessage)
            {
                _processingMessage = true;
                Context.ActorSelection("/user/leaderboard").Tell(gameView);
            }
        }
    }
}
