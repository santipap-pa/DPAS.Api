using DPAS.Api.Context;
using DPAS.Api.Models.Data;
using DPAS.Api.Enums;
using Microsoft.EntityFrameworkCore;
using DPAS.Api.Services;
using DPAS.Api.Dtos;

namespace DPAS.Api.Repositories
{
    public interface IDisasterRepository
    {
        public Task<List<GetDisasterRiskDto>> GetDisasterRiskAsync();
    }
    public class DisasterRepository : IDisasterRepository
    {
        private readonly BaseDbContext _context;
        private readonly OpenWeatherService _openWeatherService;
        private readonly USGSService _usgsService;
        private readonly CalculateRiskService _calculateRiskService;
        private readonly RedisCacheService _redisCacheService;
        private readonly ILogger<DisasterRepository> _logger;

        public DisasterRepository(BaseDbContext context, ILogger<DisasterRepository> logger
            , OpenWeatherService openWeatherService, USGSService usgsService, CalculateRiskService calculateRiskService, RedisCacheService redisCacheService)
        {
            _context = context;
            _logger = logger;
            _openWeatherService = openWeatherService;
            _usgsService = usgsService;
            _calculateRiskService = calculateRiskService;
            _redisCacheService = redisCacheService;
        }

        public async Task<List<GetDisasterRiskDto>> GetDisasterRiskAsync()
        {
            try
            {
                var disasterAlert = await _context.Alerts
                    .Include(t => t.Region)
                    .Where(t => t.IsActive == true)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                var disasterRiskList = new List<GetDisasterRiskDto>();

                foreach (var alert in disasterAlert)
                {
                    var cacheKey = $"disasterRisk_{alert.Region.RegionID}_{alert.DisasterType}";
                    var cachedData = await _redisCacheService.GetAsync<GetDisasterRiskDto>(cacheKey);
                    
                    if (cachedData != null)
                    {
                        disasterRiskList.Add(cachedData);
                    }
                    else
                    {
                        var weatherData = await _openWeatherService.GetWeatherDataAsync(alert.Region.LocationCoordinates.Latitude, alert.Region.LocationCoordinates.Longitude);
                        // var earthquakeData = await _usgsService.GetEarthquakeDataAsync();

                        var riskScore = _calculateRiskService.CalculateFloodRisk(weatherData.Rain.OneHour.Value);

                        var disasterRiskDto = new GetDisasterRiskDto
                        {
                            RegionID = alert.Region.RegionID,
                            DisasterType = alert.DisasterType.ToString(),
                            // RiskScore = riskScore,
                            RiskLevel = riskScore > 50 ? "High" : "Low",
                            LastUpdated = DateTime.UtcNow
                        };

                        await _redisCacheService.SetAsync(cacheKey, disasterRiskDto, TimeSpan.FromHours(1));
                        disasterRiskList.Add(disasterRiskDto);
                    }
                }

                return disasterRiskList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching disaster risk data");
                throw;
            }
        }
    }
}