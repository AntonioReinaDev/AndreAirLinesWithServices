using System.Net.Http;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;

namespace APIAeroporto.Service
{
    public class AirportsQuery
    {
        static readonly HttpClient client = new HttpClient();
        public static async Task<Airport> GetAirportAsync(string code)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:44315/api/Airport/" + code);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var airport = JsonConvert.DeserializeObject<Airport>(responseBody);

            return airport;
        }
    }
}
