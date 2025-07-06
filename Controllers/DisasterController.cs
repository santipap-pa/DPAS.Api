using DPAS.Api.Enums;
using DPAS.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/disaster")]
public class DisasterController : ControllerBase
{
    private readonly ILogger<DisasterController> _logger;
    private readonly IDisasterRepository _disasterRepository;

    public DisasterController(ILogger<DisasterController> logger ,IDisasterRepository disasterRepository)
    {
        _logger = logger;
        _disasterRepository = disasterRepository;
    }

    [HttpGet("risks")]
    public async Task<ActionResult>? GetDisasterRisks()
    {
        try
        {
            var disasterRisk = await _disasterRepository.GetDisasterRiskAsync();
            if (disasterRisk == null)
            {
                return NotFound("No disaster risk data found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching disaster risks");
            return StatusCode(500, "Internal server error");
        }

        return StatusCode(200, await _disasterRepository.GetDisasterRiskAsync());
    }

    [HttpGet("types")]
    public ActionResult GetDisasterTypes()
    {
        return StatusCode(200, DisasterTypeEnum.GetValues(typeof(DisasterTypeEnum))
            .Cast<DisasterTypeEnum>()
            .Select(e => new
            {
                Id = (int)e,
                Name = e.ToString()
            })
            .ToList());
    }
}