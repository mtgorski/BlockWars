using System;
using Akka.Actor;
using Akka.DI.Core;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerActor : ReceiveActor
    {
        public PlayerActor()
        {
            Receive<BuildBlockCommand>(x =>
            {
                BuildBlock(x);
                return true;
            });

            Receive<BlockBuiltMessage>(x =>
            {
                OnBlockBuilt(x);
                return true;
            });

            var statsActor = Context.ActorOf(Context.System.DI().Props<PlayerStatsActor>(), "stats");
            Context.System.EventStream.Subscribe(statsActor, typeof(LeagueEndedMessage));
        }

        private void OnBlockBuilt(BlockBuiltMessage x)
        {
            Context.Child("stats").Tell(x);
        }

        private void BuildBlock(BuildBlockCommand command)
        {
            Context.System.ActorSelection("/user/supervisor/" + command.LeagueId.ToString()).Tell(command);
        }
    }
}
