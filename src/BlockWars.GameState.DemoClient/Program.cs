using BlockWars.GameState.Client;
using BlockWars.GameState.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlockWars.GameState.DemoClient
{
    public class Program
    {
        private static Random Rng = new Random();

        public static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        public static async Task Run()
        {
            var gameClient = new GameStateClient(new System.Net.Http.HttpClient(), "http://localhost:5000");
            var leagueId = Guid.Parse("5a44f339-3133-4f79-a6dc-1862568569cc");
            while (true)
            {
                var regions = await gameClient.GetRegionsAsync(leagueId);
                var regionSelector = Rng.Next(regions.Count);
                var whichRegion = regions.Where((x, i) => i == regionSelector).Single();
                await gameClient.BuildBlockAsync(leagueId, whichRegion.RegionId);
            }
        }
    }
}
