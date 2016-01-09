using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BlockWars.GameState.Api.Automapper;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;

namespace BlockWars.GameState.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<IGetLeagues, GetLeaguesService>();
            services.AddScoped<IUpsertLeague, UpsertLeagueService>();
            services.AddScoped<IUpsertRegion, UpsertRegionService>();
            services.AddScoped<IGetRegions, GetRegionsService>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            Mapper.AddProfile<LeagueProfile>();
            Mapper.AddProfile<RegionProfile>();
            services.AddSingleton(_ => Mapper.Engine);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
            
            app.UseIISPlatformHandler();

        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
