using System.Text.Json;
using DPAS.Models.External;

namespace DPAS.Api.Services
{
    public class USGSService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<USGSService> _logger;

        public USGSService(HttpClient httpClient, ILogger<USGSService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SeismicDataResponse> GetSeismicDataAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&latitude={latitude}&longitude={longitude}&maxradiuskm=100&minmagnitude=1&limit=1&orderby=time";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var earthquakeData = JsonSerializer.Deserialize<USGSResponse>(json);

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