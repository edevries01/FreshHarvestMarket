/*
 * WeatherApiController.cs
 * FreshHarvestMarket
 *
 * This API controller provides weather data endpoints for the application.
 *
 * It handles:
 * - Retrieving current weather data from the WeatherService
 * - Returning temperature & weather condition data as JSON
 * - Serving as a bridge between the frontend & external weather API
 *
 * The controller uses attribute routing to expose API endpoints
 * (e.g., /api/weather) that can be accessed asynchronously
 * by client-side scripts or other services.
 *
 * This allows the application to dynamically fetch weather data
 * without relying solely on server-rendered components.
 */

using Microsoft.AspNetCore.Mvc;
using FreshHarvestMarket.Services;

[Route("api/[controller]")]
[ApiController]
public class WeatherApiController : ControllerBase
{
    private readonly WeatherService _weatherService;

    public WeatherApiController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWeather()
    {
        var (temp, code) = await _weatherService.GetWeatherAsync();

        return Ok(new
        {
            Temperature = temp,
            WeatherCode = code
        });
    }
}