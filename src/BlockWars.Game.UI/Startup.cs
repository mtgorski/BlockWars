using BlockWars.GameState.Client;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using BlockWars.Game.UI.Actors;
using Akka.Actor;
using BlockWars.Game.UI.IoC;
using Akka.DI.Core;
using BlockWars.Game.UI.Strategies;
using BlockWars.Game.UI.Options;

namespace BlockWars.Game.UI
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<HttpClient>();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("urls.json");
            configurationBuilder.AddJsonFile("appsettings.json");
            var config = configurationBuilder.Build();
            var gameStateUrl = config.GetSection("urls")["GameStateApi"];

            services.AddOptions();
            services.Configure<DemoOptions>(config);
            services.Configure<GameDuration>(config);

            services.AddSingleton<IGameStateClient, NullGameClient>();
            services.AddTransient<INewLeagueFactory, HardCodedLeagueFactory>();
            services.AddTransient<INewRegionsFactory, HardCodedRegionsFactory>();

            services.AddSignalR(
                o =>
                {
                    o.Hubs.EnableDetailedErrors = true;
                });

            services.AddSingleton<ActorSystem>(
                x =>
                {
                    var system = ActorSystem.Create("BlockWars");
                    system.AddDependencyResolver(new ActorContainer(x, system));
                    return system;
                });

            services.AddSingleton<IServerManager, AkkaAdapter>();

            services.AddTransient<LeagueActor, LeagueActor>();
            services.AddSingleton<ServerSupervisor, ServerSupervisor>();
            services.AddSingleton<Broadcaster, Broadcaster>();
            services.AddTransient<DemoActor, DemoActor>();
            services.AddTransient<LeaguePersistenceActor, LeaguePersistenceActor>();            
        }

        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseIISPlatformHandler();
            app.UseSignalR();
            app.UseMvc();

            app.CreateActorSystem();
        }


        public static void Main(string[] args)
        {
            WebApplication.Run<Startup>(args);
        }
    }
}
