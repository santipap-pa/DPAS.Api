using AutoMapper;
using DPAS.Api.Dtos;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using DPAS.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers;

[ApiController]
[Route("api/regions")]
public class RegionController : ControllerBase
{
    private readonly ILogger<RegionController> _logger;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionController(ILogger<RegionController> logger, IRegionRepository regionRepository, IMapper mapper)
    {
        _logger = logger;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResultModel<GetRegionDto>>> GetAllRegions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _regionRepository.GetAllAsync(pageNumber, pageSize);
            var regionDtos = _mapper.Map<List<GetRegionDto>>(result.Items);

            return StatusCode(200, PaginatedResultModel<GetRegionDto>.Paginated(regionDtos, result.TotalCount, result.PageNumber, result.PageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting regions");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{regionId}")]
    public async Task<ActionResult<GetRegionDto>> GetRegionById(string regionId)
    {
        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(regionId);
            if (region == null)
            {
                return StatusCode(404);
            }

            return Ok(_mapper.Map<GetRegionDto>(region));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting region by region id {RegionId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetRegionDto>> CreateRegion([FromBody] CreateRegionDto request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, ModelState);
        }

        try
        {
            if (await _regionRepository.ExistsByRegionIdAsync(request.RegionID))
            {
                return StatusCode(400, "Region with this RegionID already exists");
            }

            var region = _mapper.Map<RegionModel>(request);
            var createdRegion = await _regionRepository.CreateAsync(region);
            var regionDto = _mapper.Map<GetRegionDto>(createdRegion);

            return StatusCode(201, regionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating region");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{regionsId}")]
    public async Task<ActionResult<GetRegionDto>> UpdateRegion(string regionId, [FromBody] UpdateRegionDto request)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(400, ModelState);
        }

        try
        {
            var existingRegion = await _regionRepository.GetByRegionIdAsync(regionId);
            if (existingRegion == null)
            {
                return NotFound();
            }

            _mapper.Map(request, existingRegion);
            var updatedRegion = await _regionRepository.UpdateAsync(existingRegion);

            return Ok(_mapper.Map<GetRegionDto>(updatedRegion));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating region {RegionId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{regionId}")]
    public async Task<ActionResult> DeleteRegion(string regionId)
    {
        try
        {
            var deleted = await _regionRepository.DeleteByRegionIdAsync(regionId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting region {RegionId}", regionId);
            return StatusCode(500, "Internal server error");
        }
    }
}
