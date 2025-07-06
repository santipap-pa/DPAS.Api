using DPAS.Api.Context;
using DPAS.Api.Models.Data;
using DPAS.Api.Enums;
using Microsoft.EntityFrameworkCore;
using DPAS.Api.Services;
using DPAS.Api.Dtos;
using DPAS.Api.Extensions;

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
                        var disasterRiskDto = await CreateDisasterRiskDto(alert);
                        if (disasterRiskDto != null)
                        {
                            await _redisCacheService.SetAsync(cacheKey, disasterRiskDto, TimeSpan.FromMinutes(1));
                            disasterRiskList.Add(disasterRiskDto);
                        }
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

        private async Task<GetDisasterRiskDto?> CreateDisasterRiskDto(AlertModel alert)
        {

            switch (alert.DisasterType)
            {
                case DisasterTypeEnum.Wildfire:
                    var wildfireWeatherData = await _openWeatherService.GetWeatherDataAsync(
                        alert.Region.LocationCoordinates.Latitude,
                        alert.Region.LocationCoordinates.Longitude);

                    var wildfireRiskScore = _calculateRiskService.CalculateWildfireRisk(wildfireWeatherData?.Main?.Temp ?? 0.00, wildfireWeatherData?.Main?.Humidity ?? 0.00);
                    
                    var wildfireRiskLevel = CalculateRiskLevel(wildfireRiskScore, alert.ThresholdScore);

                    return new GetDisasterRiskDto
                    {
                        RegionID = alert.Region.RegionID,
                        DisasterType = alert.DisasterType.ToString(),
                        RiskScore = wildfireRiskScore,
                        RiskLevel = wildfireRiskLevel.GetDisplayName(),
                        AlertTrigger = wildfireRiskLevel == RiskLevelEnum.High,
                    };

                case DisasterTypeEnum.Flood:
                    var floodWeatherData = await _openWeatherService.GetWeatherDataAsync(
                        alert.Region.LocationCoordinates.Latitude,
                        alert.Region.LocationCoordinates.Longitude);

                    var floodRiskScore = _calculateRiskService.CalculateFloodRisk(floodWeatherData?.Rain?.OneHour ?? 0.00);

                    var floodRiskLevel = CalculateRiskLevel(floodRiskScore, alert.ThresholdScore);

                    return new GetDisasterRiskDto
                    {
                        RegionID = alert.Region.RegionID,
                        DisasterType = alert.DisasterType.ToString(),
                        RiskScore = floodRiskScore,
                        RiskLevel = floodRiskLevel.GetDisplayName(),
                        AlertTrigger = floodRiskLevel == RiskLevelEnum.High,
                    };

                case DisasterTypeEnum.Earthquake:
                    var earthquakeData = await _usgsService.GetSeismicDataAsync(alert.Region.LocationCoordinates.Latitude,
                        alert.Region.LocationCoordinates.Longitude);
                    var earthquakeRiskScore = _calculateRiskService.CalculateEarthquakeRisk(earthquakeData.Magnitude);

                    var earthquakeRiskLevel = CalculateRiskLevel(earthquakeRiskScore, alert.ThresholdScore);

                    return new GetDisasterRiskDto
                    {
                        RegionID = alert.Region.RegionID,
                        DisasterType = alert.DisasterType.ToString(),
                        RiskScore = earthquakeRiskScore,
                        RiskLevel = earthquakeRiskLevel.GetDisplayName(),
                        AlertTrigger = earthquakeRiskLevel == RiskLevelEnum.High,
                    };

                default:
                    _logger.LogWarning("Unsupported disaster type: {DisasterType}", alert.DisasterType);
                    return null;
            }
        }

        private RiskLevelEnum CalculateRiskLevel(int riskScore, int thresholdScore)
        {
            if (riskScore >= thresholdScore)
                return RiskLevelEnum.High;
            else if (riskScore >= thresholdScore * 0.5)
                return RiskLevelEnum.Medium;
            else
                return RiskLevelEnum.Low;
        }
    }
}