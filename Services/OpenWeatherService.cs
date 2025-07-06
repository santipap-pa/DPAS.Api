using System.Text.Json;
using DPAS.Models.External;

namespace DPAS.Api.Services
{
    public class  OpenWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenWeatherService> _logger;
        private readonly string _apiKey;

        public OpenWeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenWeatherService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _apiKey = _configuration["OpenWeather:ApiKey"] ?? throw new InvalidOperationException("OpenWeather ApiKey is missing");
        }

        public async Task<WeatherApiResponse> GetWeatherDataAsync(double latitude, double longitude)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            return weatherData ?? throw new InvalidOperationException("Failed to deserialize weather data");
        }
    }
}