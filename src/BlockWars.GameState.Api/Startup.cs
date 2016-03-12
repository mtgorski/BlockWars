using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BlockWars.GameState.Api.Automapper;
using BlockWars.GameState.Api.Repositories;
using BlockWars.GameState.Api.Services;
using BlockWars.GameState.Api.Filters;
using BlockWars.GameState.Api.Validators;
using FluentValidation;
using BlockWars.GameState.Models;
using BlockWars.GameState.Api.Validators.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BlockWars.GameState.Api
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("urls.json");
            var config = configurationBuilder.Build();
            var gameStateDatabase = config.GetSection("urls")["GameStateDatabase"];

            services.AddSingleton(_ => new MongoClient(gameStateDatabase));
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<IGetLeagues, GetLeaguesService>();
            services.AddScoped<IUpsertLeague, UpsertLeagueService>();
            services.AddScoped<IUpsertRegion, UpsertRegionService>();
            services.AddScoped<IGetRegions, GetRegionsService>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ValidateLeagueFilter, ValidateLeagueFilter>();
            services.AddScoped<AbstractValidator<League>, LeagueValidator>();
            services.AddScoped<ValidateLeagueIdFilter, ValidateLeagueIdFilter>();
            services.AddScoped<IValidateLeagueId, LeagueRepository>();
            services.AddScoped<ValidateRegionFilter, ValidateRegionFilter>();
            services.AddScoped<AbstractValidator<Region>, RegionValidator>();

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
