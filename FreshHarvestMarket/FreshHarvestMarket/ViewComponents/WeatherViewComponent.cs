/*
 * WeatherViewComponent.cs
 * FreshHarvestMarket
 *
 * This ViewComponent is responsible for retrieving & displaying
 * current weather information on the application UI.
 *
 * It uses the WeatherService to fetch temperature and weather condition
 * data, then generates a user-friendly message based on the results.
 *
 * The output is displayed in the shared layout (footer) to inform users
 * about weather conditions that may affect pickup plans.
 */

using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Services;

namespace FreshHarvestMarket.ViewComponents
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly WeatherService _weatherService;

        public WeatherViewComponent(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var (temp, code) = await _weatherService.GetWeatherAsync();

            string message = GetMessage(temp, code);

            ViewBag.Temp = temp;
            ViewBag.Message = message;

            return View();
        }

        private string GetMessage(double temp, int code)
        {
            if (code >= 95) return "⚠️ Severe storm expected — plan your pickup carefully!";
            if (code >= 61) return "🌧️ Rainy conditions — bring an umbrella!";
            if (temp > 75) return "☀️ Great day to pick up fresh produce!";
            if (temp < 40) return "🥶 Cold day — dress warm for pickup!";

            return "🌤️ Decent weather for your market trip!";
        }
    }
}