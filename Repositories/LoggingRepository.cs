using DPAS.Api.Context;
using DPAS.Api.Extensions;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;

namespace DPAS.Api.Repositories
{
    public interface ILoggingRepository
    {
        public Task<PaginatedResultModel<LoggingModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    }
    public class LoggingRepository : ILoggingRepository
    {
        private readonly BaseDbContext _context;

        public LoggingRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResultModel<LoggingModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Logs.AsQueryable();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }
    }


}