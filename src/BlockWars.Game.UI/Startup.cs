using BlockWars.GameState.Client;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace BlockWars.Game.UI
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IGameStateClient>(x => new GameStateClient(x.GetService<HttpClient>(), "http://localhost:5000"));
            services.AddTransient<IGameManagerProvider, GameManagerProvider>();
            services.AddTransient<INewInstanceFactory, NewInstanceFactory>();
            services.AddTransient<INewLeagueStrategy, HardCodedLeagueStrategy>();
            services.AddTransient<INewRegionsStrategy, HardCodedRegionsStrategy>();
            services.AddSingleton<ServerManager>();

            services.AddSignalR(
                o =>
                {
                    o.Hubs.EnableDetailedErrors = true;
                });

            ServiceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            app.UseStaticFiles();
            app.UseMvc();
            app.UseIISPlatformHandler();

            app.UseSignalR();
        }

        // Entry point for the application.
        public static void Main(string[] args)
        {
            WebApplication.Run<Startup>(args);
        }
    }
}
