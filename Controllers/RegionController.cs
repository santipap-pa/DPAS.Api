using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/regions")]
public class RegionController : ControllerBase
{
    private readonly ILogger<RegionController> _logger;

    public RegionController(ILogger<RegionController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CreateRegion()
    {
       return Ok();
    }
}
