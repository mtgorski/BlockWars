using Akka.Actor;
using Akka.DI.Core;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerSupervisor : ReceiveActor
    {
        public PlayerSupervisor()
        {
            Receive<UserConnectedMessage>(x =>
            {
                OnUserConnected(x);
                return true;
            });

            Receive<UserDisconnectedMessage>(x =>
            {
                OnUserDisconnected(x);
                return true;
            });

        }

        private void OnUserDisconnected(UserDisconnectedMessage x)
        {
            Context.Child("player" + x.ConnectionId).Tell(PoisonPill.Instance);
        }

        private void OnUserConnected(UserConnectedMessage x)
        {
            var statsActor = Context.ActorOf(Context.System.DI().Props<PlayerActor>(), "player" + x.ConnectionId);
        }
    }
}
