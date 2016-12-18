using Akka.Actor;
using BlockWars.Game.UI.Actors;
using Microsoft.AspNetCore.Builder;
using Akka.DI.Core;
using Microsoft.Extensions.DependencyInjection;
using BlockWars.Game.UI.ViewModels;

namespace BlockWars.Game.UI
{
    public static class ApplicationBuilderExtensions
    {
        public static void CreateActorSystem(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices;
            var actorSystem = provider.GetService<ActorSystem>();
            var supervisor = actorSystem.ActorOf(actorSystem.DI().Props<ServerSupervisor>(), "supervisor");
            var broadcaster = actorSystem.ActorOf(actorSystem.DI().Props<Broadcaster>(), "broadcaster");
            var demo = actorSystem.ActorOf(actorSystem.DI().Props<DemoActor>(), "demo");
            var saver = actorSystem.ActorOf(actorSystem.DI().Props<GamePersistenceActor>(), "saver");
            var statsSupervisor = actorSystem.ActorOf(actorSystem.DI().Props<PlayerSupervisor>(), "playerSupervisor");
            var leaderboard = actorSystem.ActorOf(actorSystem.DI().Props<LeaderboardActor>(), "leaderboard");
            var leaderboardBuffer = actorSystem.ActorOf(actorSystem.DI().Props<LeaderboardBuffer>(), "leaderboardBuffer");

            actorSystem.EventStream.Subscribe(broadcaster, typeof(GameViewModel));
            actorSystem.EventStream.Subscribe(demo, typeof(GameViewModel));
            actorSystem.EventStream.Subscribe(leaderboardBuffer, typeof(GameViewModel));

            actorSystem.EventStream.Subscribe(broadcaster, typeof(GameEndedMessage));
            actorSystem.EventStream.Subscribe(saver, typeof(GameEndedMessage));
            actorSystem.EventStream.Subscribe(supervisor, typeof(GameEndedMessage));
            actorSystem.EventStream.Subscribe(leaderboardBuffer, typeof(GameEndedMessage));

            actorSystem.EventStream.Subscribe(statsSupervisor, typeof(UserConnectedMessage));
            actorSystem.EventStream.Subscribe(statsSupervisor, typeof(UserDisconnectedMessage));
        }
    }
}
