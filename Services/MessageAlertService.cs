namespace DPAS.Api.Services
{
    public class MessageAlertService
    {
        private readonly ILogger<MessageAlertService> _logger;

        public MessageAlertService(ILogger<MessageAlertService> logger)
        {
            _logger = logger;
        }

        public void SendAlert(string message)
        {
            //Mockup
            _logger.LogInformation($"Alert sent: {message}");
        }
    }
}