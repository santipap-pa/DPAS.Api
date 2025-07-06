using DPAS.Api.Context;
using DPAS.Api.Models.Data;
using DPAS.Api.Enums;
using Microsoft.EntityFrameworkCore;
using DPAS.Api.Services;

namespace DPAS.Api.Repositories
{
    public interface IDisasterRepository
    {
      
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
    }
}