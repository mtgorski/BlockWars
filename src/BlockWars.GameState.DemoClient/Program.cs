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

        public static Task Run()
        {
            //TODO: rewrite to work with new in memory state
            return Task.FromResult(0);
        }
    }
}
