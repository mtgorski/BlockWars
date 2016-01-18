using BlockWars.GameState.Client;
using System;
using System.Linq;
using System.Net.Http;
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

        public async static Task Run()
        {
            var client = new HttpClient();
            while(true)
            {
               var response = await client.PostAsync("http://localhost:49873/api/demo/build_block", new StringContent("content"));
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
