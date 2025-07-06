namespace DPAS.Api.Services
{
    public class MessageAlertService
    {
        private readonly ILogger<MessageAlertService> _logger;
        private readonly LoggingService _loggingService;

        public MessageAlertService(ILogger<MessageAlertService> logger, LoggingService loggingService)
        {
            _logger = logger;
            _loggingService = loggingService;
        }

        public async void SendAlert(string message)
        {
            //Mockup
            await _loggingService.LogAlertAsync("SendAlert", message);
            _logger.LogInformation($"Alert sent: {message}");
        }
    }
}