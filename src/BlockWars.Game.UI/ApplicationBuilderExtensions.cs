using Akka.Actor;
using BlockWars.Game.UI.Actors;
using Microsoft.AspNet.Builder;
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
            actorSystem.ActorOf(actorSystem.DI().Props<ServerSupervisor>(), "supervisor");
            var broadcaster = actorSystem.ActorOf(actorSystem.DI().Props<Broadcaster>(), "broadcaster");
            var demo = actorSystem.ActorOf(actorSystem.DI().Props<DemoActor>(), "demo");

            var registry = provider.GetService<ISubscriptionRegistry>();
            registry.Subscribe<LeagueViewModel>(broadcaster);
            registry.Subscribe<LeagueViewModel>(demo);
        }
    }
}
