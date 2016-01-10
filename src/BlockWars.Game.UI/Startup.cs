using BlockWars.GameState.Client;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;

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
            var config = configurationBuilder.Build();
            var gameStateUrl = config.GetSection("urls")["GameStateApi"];

            services.AddSingleton<IGameStateClient>(x => new GameStateClient(x.GetService<HttpClient>(), gameStateUrl));
            services.AddTransient<IGameManagerProvider, GameManagerProvider>();
            services.AddTransient<INewInstanceFactory, NewInstanceFactory>();
            services.AddTransient<INewLeagueStrategy, HardCodedLeagueStrategy>();
            services.AddTransient<INewRegionsStrategy, HardCodedRegionsStrategy>();
            services.AddSingleton<IServerManager, ServerManager>();


            services.AddSignalR(
                o =>
                {
                    o.Hubs.EnableDetailedErrors = true;
                });
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
