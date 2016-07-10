using System;
using Akka.Actor;
using Akka.DI.Core;

namespace BlockWars.Game.UI.Actors
{
    public class UserStatsSupervisorActor : ReceiveActor
    {
        public UserStatsSupervisorActor()
        {
            Receive<UserConnectedMessage>(x =>
            {
                OnUserConnected(x);
                return true;
            });

            Receive<BlockBuiltMessage>(x =>
            {
                OnBlockBuiltMessage(x);
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
            Context.Child("stats" + x.ConnectionId).Tell(PoisonPill.Instance);
        }

        private void OnBlockBuiltMessage(BlockBuiltMessage x)
        {
            Context.Child("stats" + x.ConnectionId).Tell(x);
        }

        private void OnUserConnected(UserConnectedMessage x)
        {
            var statsActor = Context.ActorOf(Context.System.DI().Props<UserStatsActor>(), "stats" + x.ConnectionId);
            Context.System.EventStream.Subscribe(statsActor, typeof(LeagueEndedMessage));
        }
    }
}
