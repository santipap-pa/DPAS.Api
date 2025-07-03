using AutoMapper;
using DPAS.Api.Dtos;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using DPAS.Api.Repositories;
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
            
            return Ok(PaginatedResultModel<GetRegionDto>.Paginated(regionDtos, result.TotalCount, result.PageNumber, result.PageSize));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting regions");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{regionsId}")]
    public async Task<ActionResult<GetRegionDto>> GetRegionById(string regionsId)
    {
        try
        {
            var region = await _regionRepository.GetByRegionIdAsync(regionsId);
            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetRegionDto>(region));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting region by region id {RegionId}", regionsId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetRegionDto>> CreateRegion([FromBody] CreateRegionDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            if (await _regionRepository.ExistsByRegionIdAsync(request.RegionID))
            {
                return Conflict("Region with this RegionID already exists");
            }

            var region = _mapper.Map<RegionModel>(request);
            var createdRegion = await _regionRepository.CreateAsync(region);
            var regionDto = _mapper.Map<GetRegionDto>(createdRegion);

            return CreatedAtAction(nameof(GetRegionById), new { regionsId = createdRegion.RegionID }, regionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating region");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{regionsId}")]
    public async Task<ActionResult<GetRegionDto>> UpdateRegion(string regionsId, [FromBody] UpdateRegionDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var existingRegion = await _regionRepository.GetByRegionIdAsync(regionsId);
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
            _logger.LogError(ex, "Error updating region {RegionId}", regionsId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{regionsId}")]
    public async Task<ActionResult> DeleteRegion(string regionsId)
    {
        try
        {
            var deleted = await _regionRepository.DeleteByRegionIdAsync(regionsId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting region {RegionId}", regionsId);
            return StatusCode(500, "Internal server error");
        }
    }
}
