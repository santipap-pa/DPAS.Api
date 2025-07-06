using AutoMapper;
using DPAS.Api.Dtos;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using DPAS.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly ILogger<AlertController> _logger;
    private readonly IAlertRepository _alertRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public AlertController(ILogger<AlertController> logger, IAlertRepository alertRepository, IRegionRepository regionRepository, IMapper mapper)
    {
        _logger = logger;
        _alertRepository = alertRepository;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResultModel<GetAlertDto>>> GetAlerts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _alertRepository.GetAllAsync(pageNumber, pageSize);
            var alertDtos = _mapper.Map<List<GetAlertDto>>(result.Items);

            return StatusCode(200, PaginatedResultModel<GetAlertDto>.Paginated(alertDtos, result.TotalCount, result.PageNumber, result.PageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting alerts");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{regionId}")]
    public async Task<ActionResult<GetAlertDto>> GetAlertById(string regionId)
    {
        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(regionId);
            if (region == null)
            {
                return StatusCode(404);
            }

            var alert = await _alertRepository.GetByIdAsync(region.Id);
            if (alert == null)
            {
                return StatusCode(404);
            }

            return Ok(_mapper.Map<GetAlertDto>(alert));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting alert by id {AlertId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetAlertDto>> CreateAlert([FromBody] CreateAlertDto request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, ModelState);
        }

        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(request.RegionID);
            if (region == null)
            {
                return StatusCode(400, $"RegionID '{request.RegionID}' does not exist.");
            }

            var alert = _mapper.Map<AlertModel>(request);
            alert.RegionId = region.Id;

            var createdAlert = await _alertRepository.CreateAsync(alert);
            var alertDto = _mapper.Map<GetAlertDto>(createdAlert);

            return StatusCode(201, alertDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating alert");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{regionId}")]
    public async Task<ActionResult<GetAlertDto>> UpdateAlert(string regionId, [FromBody] UpdateAlertDto request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, ModelState);
        }

        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(regionId);
            if (region == null)
            {
                return StatusCode(400, $"RegionID '{regionId}' does not exist.");
            }

            var existingAlert = await _alertRepository.GetByIdAsync(region.Id);
            if (existingAlert == null)
            {
                return StatusCode(404);
            }

            _mapper.Map(request, existingAlert);
            var updatedAlert = await _alertRepository.UpdateAsync(existingAlert);

            return Ok(_mapper.Map<GetAlertDto>(updatedAlert));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating alert {RegionId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{regionId}")]
    public async Task<ActionResult> DeleteAlert(string regionId)
    {
        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(regionId);
            if (region == null)
            {
                return StatusCode(400, $"RegionID '{regionId}' does not exist.");
            }

            var deleted = await _alertRepository.DeleteAsync(region.Id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting alert {RegionId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }
}
