using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using BlockWars.Game.UI.Options;
using BlockWars.Game.UI;
using BlockWars.GameState.Client;
using BlockWars.Game.UI.Strategies;
using BlockWars.Game.UI.IoC;
using Akka.Actor;
using BlockWars.Game.UI.Actors;
using Akka.DI.Core;
using Microsoft.Framework.Configuration;

namespace BlockWars.GameServer.UI
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("urls.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public Microsoft.Extensions.Configuration.IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<HttpClient>();

            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            configurationBuilder.SetBasePath(_env.ContentRootPath);
            configurationBuilder.AddJsonFile("urls.json");
            configurationBuilder.AddJsonFile("appsettings.json");
            var config = configurationBuilder.Build();
            var gameStateUrl = config.GetSection("urls")["GameStateApi"];

            services.AddOptions();
            services.Configure<DemoOptions>(config);
            services.Configure<GameDuration>(config);

            services.AddSingleton<IGameStateClient, NullGameClient>();
            services.AddTransient<INewGameFactory, HardCodedGameFactory>();
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
            services.AddTransient<AccomplishmentManager, AccomplishmentManager>();

            services.AddTransient<GameActor, GameActor>();
            services.AddSingleton<ServerSupervisor, ServerSupervisor>();
            services.AddSingleton<Broadcaster, Broadcaster>();
            services.AddTransient<DemoActor, DemoActor>();
            services.AddTransient<GamePersistenceActor, GamePersistenceActor>();
            services.AddTransient<PlayerStatsActor, PlayerStatsActor>();
            services.AddSingleton<PlayerSupervisor, PlayerSupervisor>();
            services.AddTransient<PlayerActor, PlayerActor>();
            services.AddSingleton<LeaderboardActor, LeaderboardActor>();
            services.AddSingleton<LeaderboardBuffer, LeaderboardBuffer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
           // app.UseIISPlatformHandler();
            app.UseSignalR();
            app.UseMvc();

            app.CreateActorSystem();
        }
    }
}
