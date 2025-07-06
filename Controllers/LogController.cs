using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using DPAS.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DPAS.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly ILoggingRepository _loggingRepository;

        public LogController(ILogger<LogController> logger, ILoggingRepository loggingRepository)
        {
            _logger = logger;
            _loggingRepository = loggingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResultModel<LoggingModel>>> GetLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _loggingRepository.GetAllAsync(pageNumber, pageSize);

                return StatusCode(200, PaginatedResultModel<LoggingModel>.Paginated(result.Items, result.TotalCount, result.PageNumber, result.PageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting logs");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}