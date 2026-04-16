using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreshHarvestMarket.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double temp, int weatherCode)> GetWeatherAsync()
        {
            string url =
                "https://api.open-meteo.com/v1/forecast?latitude=41.6611&longitude=-92.0096&current_weather=true&temperature_unit=fahrenheit";

            var response = await _httpClient.GetStringAsync(url);
            var data = JsonDocument.Parse(response);

            var current = data.RootElement.GetProperty("current_weather");

            double temp = current.GetProperty("temperature").GetDouble();
            int weatherCode = current.GetProperty("weathercode").GetInt32();

            return (temp, weatherCode);
        }
    }
}