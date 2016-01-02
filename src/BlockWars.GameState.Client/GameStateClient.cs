using BlockWars.GameState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace BlockWars.GameState.Client
{
    public class GameStateClient : IGameStateClient
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public GameStateClient(HttpClient client, string baseUrl)
        {
            _client = client;
            _baseUrl = baseUrl;
        }

        public async Task<League> GetCurrentLeagueAsync()
        {
            var response = await _client.GetAsync(_baseUrl + "/api/leagues?isCurrent=true");
            response.EnsureSuccessStatusCode();
            var leaguesResponse = await response.Content.ReadAsAsync<LeaguesResponse>();
            return leaguesResponse.Leagues.FirstOrDefault();
        }

        public async Task PutLeagueAsync(Guid leagueId, League league)
        {
            var response = await _client.PutAsync(_baseUrl + "/api/leagues/" + leagueId, league, new JsonMediaTypeFormatter());
            response.EnsureSuccessStatusCode();
        }

        public async Task<ICollection<Region>> GetRegionsAsync(Guid leagueId)
        {
            var response = await _client.GetAsync(_baseUrl + "/api/leagues/" + leagueId + "/regions");
            response.EnsureSuccessStatusCode();
            var regionsResponse = await response.Content.ReadAsAsync<RegionsResponse>();
            return regionsResponse.Regions;
        }

        public async Task PutRegionAsync(Guid leagueId, Guid regionId, Region region)
        {
            var response = await _client.PutAsync(_baseUrl + "/api/leagues/" + leagueId + "/regions/" + regionId, region, new JsonMediaTypeFormatter());
            response.EnsureSuccessStatusCode();
        }

        public async Task BuildBlockAsync(Guid leagueId, Guid regionId)
        {
            var response = await _client.PostAsJsonAsync(
                _baseUrl + "/api/leagues/" + leagueId + "/regions/" + regionId + "/build_block", new { });
            response.EnsureSuccessStatusCode();
        }
    }
}
