using DPAS.Api.Context;
using DPAS.Api.Models.Data;

namespace DPAS.Api.Services
{
    public class LoggingService
    {
        private readonly BaseDbContext _context;

        public LoggingService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task LogApiUsageAsync(string endpoint, string httpMethod, int statusCode, long responseTime)
        {
            var log = new LoggingModel
            {
                LogType = "API_USAGE",
                Action = $"{httpMethod} {endpoint}",
                Description = $"API call to {endpoint} completed with status {statusCode}",
                Endpoint = endpoint,
                HttpMethod = httpMethod,
                ResponseStatusCode = statusCode,
                ResponseTime = responseTime
            };

            await SaveLogAsync(log);
        }

        public async Task LogAlertAsync(string action, string description)
        {
            var log = new LoggingModel
            {
                LogType = "ALERT",
                Action = action,
                Description = description,
            };

            await SaveLogAsync(log);
        }

        private async Task SaveLogAsync(LoggingModel log)
        {
            try
            {
                _context.Logs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save log: {ex.Message}");
            }
        }
    }
}