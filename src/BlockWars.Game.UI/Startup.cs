using BlockWars.GameState.Client;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using BlockWars.Game.UI.Actors;

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

            services.AddSingleton<IGameStateClient>(x => new GameStateClient(x.GetService<HttpClient>(), gameStateUrl));
            services.AddTransient<INewLeagueStrategy, HardCodedLeagueStrategy>();
            services.AddTransient<INewRegionsStrategy, HardCodedRegionsStrategy>();

            services.AddSignalR(
                o =>
                {
                    o.Hubs.EnableDetailedErrors = true;
                });

            services.AddSingleton<IServerManager, AkkaAdapter>();
            services.AddTransient<LeagueActor, LeagueActor>();
            services.AddTransient<ServerSupervisor, ServerSupervisor>();
            services.AddTransient<Broadcaster, Broadcaster>();
        }

        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            
            app.UseIISPlatformHandler();
            app.UseSignalR();
            app.UseMvc();
        }

        
        public static void Main(string[] args)
        {
            WebApplication.Run<Startup>(args);
        }
    }
}
