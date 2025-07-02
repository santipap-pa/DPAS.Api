using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly ILogger<AlertsController> _logger;

    public AlertsController(ILogger<AlertsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAlerts()
    {
        return Ok();
    }

    [HttpPost("settings")]
    public IActionResult CreateAlertSettings()
    {
        return Ok();
    }

    [HttpPost("send")]
    public IActionResult SendAlert()
    {
        return Ok();
    }
}
