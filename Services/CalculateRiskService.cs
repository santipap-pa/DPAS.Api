namespace DPAS.Api.Services
{
    public class CalculateRiskService
    {
        private readonly ILogger<CalculateRiskService> _logger;

        public CalculateRiskService(ILogger<CalculateRiskService> logger)
        {
            _logger = logger;
        }


        public int CalculateFloodRisk(double rainfall)
        {

            return rainfall switch
            {
                >= 100 => 100,
                >= 75 => 85,
                >= 50 => 70,
                >= 25 => 45,
                >= 10 => 25,
                _ => 5
            };
        }

        public int CalculateEarthquakeRisk(double magnitude)
        {
            return magnitude switch
            {
                >= 7.0 => 100,
                >= 6.0 => 85,
                >= 5.0 => 70,
                >= 4.0 => 45,
                >= 3.0 => 25,
                _ => 5
            };
        }

        public int CalculateWildfireRisk(double temperature, double humidity)
        {
            var temperatureRisk = temperature switch
            {
                >= 40 => 50,
                >= 35 => 40,
                >= 30 => 25,
                >= 25 => 15,
                _ => 5
            };

            var humidityRisk = humidity switch
            {
                <= 20 => 50,
                <= 30 => 40,
                <= 40 => 25,
                <= 60 => 15,
                _ => 5
            };

            var combinedRisk = (temperatureRisk + humidityRisk) * 1.2;
            return (int)Math.Min(combinedRisk, 100);
        }
    }
}