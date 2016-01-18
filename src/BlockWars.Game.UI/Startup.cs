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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
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

            if (bool.Parse(config.GetSection("Implementation")["UseAkka"]))
            {
                ConfigureAkka(services);
            }
            else
            {
                ConfigureDefaultServices(services);
            }
            

            
        }

        private void ConfigureAkka(IServiceCollection services)
        {
            services.AddSingleton<IServerManager, AkkaAdapter>();
            services.AddTransient<LeagueActor, LeagueActor>();
            services.AddTransient<ServerSupervisor, ServerSupervisor>();
            services.AddTransient<Broadcaster, Broadcaster>();
            
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddTransient<IGameManagerProvider, GameManagerProvider>();
            services.AddTransient<INewInstanceFactory, NewInstanceFactory>();
            services.AddSingleton<IServerManager, ServerManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        // Entry point for the application.
        public static void Main(string[] args)
        {
            WebApplication.Run<Startup>(args);
        }
    }
}
