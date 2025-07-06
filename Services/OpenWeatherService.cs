using System.Text.Json;
using DPAS.Models.External;

namespace DPAS.Api.Services
{
    public class OpenWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenWeatherService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly LoggingService _loggingService;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public OpenWeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenWeatherService> logger, 
            LoggingService loggingService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _loggingService = loggingService;
            _apiKey = _configuration["OpenWeather:ApiKey"] ?? throw new InvalidOperationException("OpenWeather ApiKey is missing");
            _baseUrl = _configuration["OpenWeather:BaseUrl"] ?? throw new InvalidOperationException("OpenWeather BaseUrl is missing");
        }

        public async Task<WeatherApiResponse?> GetWeatherDataAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
        {
            var url = $"{_baseUrl}weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";

            try
            {
                _logger.LogInformation("Fetching weather data for {Latitude}, {Longitude}", latitude, longitude);
                
                var response = await _httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                
                var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(json, JsonOptions);

                await _loggingService.LogApiUsageAsync(url, "GET", (int)response.StatusCode, response.Content.Headers.ContentLength ?? 0);
                _logger.LogInformation("Weather data retrieved for {Latitude}, {Longitude}", latitude, longitude);
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error fetching weather data");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Request cancelled for {Latitude}, {Longitude}", latitude, longitude);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON error for {Latitude}, {Longitude}", latitude, longitude);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error for {Latitude}, {Longitude}", latitude, longitude);
                return null;
            }
        }
    }
}