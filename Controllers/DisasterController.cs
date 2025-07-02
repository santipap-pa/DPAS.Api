using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/disaster")]
public class DisasterController : ControllerBase
{
    private readonly ILogger<DisasterController> _logger;

    public DisasterController(ILogger<DisasterController> logger)
    {
        _logger = logger;
    }

    [HttpGet("risks")]
    public IActionResult GetDisasterRisks()
    {
       return Ok();
    }
}