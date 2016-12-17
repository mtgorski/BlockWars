using System;
using Akka.Actor;
using Akka.DI.Core;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace BlockWars.Game.UI.Actors
{
    public class PlayerActor : ReceiveActor
    {
        private readonly IConnectionManager _connectionManager;
        private string _name;

        public PlayerActor(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

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

            Receive<ChangeNameCommand>(x =>
            {
                ChangeName(x);
                return true;
            });

            Receive<NameRejectedMessage>(x =>
            {
                OnNameRejected(x);
                return true;
            });

            Receive<GameEndedMessage>(x =>
            {
                Context.Child("stats").Forward(x);
                return true;
            });

            var statsActor = Context.ActorOf(Context.System.DI().Props<PlayerStatsActor>(), "stats");

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(2),
                TimeSpan.FromMilliseconds(14),
                statsActor,
                new BroadcastCommand(),
                Context.Self);

        }

        private void ChangeName(ChangeNameCommand x)
        {
            _name = x.Name;
            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.Client(x.ConnectionId).updateName(new { Name = x.Name, Approved = true });
        }

        private void OnNameRejected(NameRejectedMessage nameRejected)
        {
            var hub = _connectionManager.GetHubContext<GameHub>();
            hub.Clients.Client(nameRejected.ConnectionId).updateName(new { Name = nameRejected.Name, Approved = false, Reason = nameRejected.Reason });
        }

        private void OnBlockBuilt(BlockBuiltMessage x)
        {
            Context.Child("stats").Tell(x);
        }

        private void BuildBlock(BuildBlockCommand command)
        {
            Context.System.ActorSelection("/user/supervisor/" + command.GameId.ToString()).Tell(command);
        }
    }
}
