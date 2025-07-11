using System.Text.Json;
using DPAS.Models.External;

namespace DPAS.Api.Services
{
    public class USGSService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<USGSService> _logger;
        private readonly LoggingService _loggingService;
        private readonly string _baseUrl;

        public USGSService(HttpClient httpClient, ILogger<USGSService> logger, IConfiguration configuration, LoggingService loggingService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _loggingService = loggingService;
            _baseUrl = _configuration["USGS:BaseUrl"] ?? throw new InvalidOperationException("USGS BaseUrl is missing");
        }

        public async Task<SeismicDataResponse> GetSeismicDataAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{_baseUrl}query?format=geojson&latitude={latitude}&longitude={longitude}&maxradiuskm=100&minmagnitude=1&limit=1&orderby=time";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var earthquakeData = JsonSerializer.Deserialize<USGSResponse>(json);

                await _loggingService.LogApiUsageAsync(url, "GET", (int)response.StatusCode, response.Content.Headers.ContentLength ?? 0);
                _logger.LogInformation("Seismic data retrieved for {Latitude}, {Longitude}", latitude,longitude);
                return new SeismicDataResponse
                {
                    Magnitude = earthquakeData?.Features?.FirstOrDefault()?.Properties?.Mag ?? 0.0
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve seismic data, defaulting to 0");
                return new SeismicDataResponse { Magnitude = 0.0 };
            }
        }
    }
}