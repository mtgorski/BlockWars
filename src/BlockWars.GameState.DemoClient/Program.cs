using BlockWars.GameState.Client;
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
            while (true)
            {
                var league = await gameClient.GetCurrentLeagueAsync();
                if(league == null)
                {
                    continue;
                }
                var leagueId = league.LeagueId;

                var regions = await gameClient.GetRegionsAsync(leagueId);
                if(regions.Count == 0)
                {
                    continue;
                }
                var regionSelector = Rng.Next(regions.Count);
                var whichRegion = regions.Where((x, i) => i == regionSelector).Single();
                await gameClient.BuildBlockAsync(leagueId, whichRegion.RegionId);
            }
        }
    }
}
