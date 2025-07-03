using DPAS.Api.Context;
using DPAS.Api.Extensions;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Repositories
{
    public interface IRegionRepository
    {
        Task<RegionModel?> GetByIdAsync(Guid id);
        Task<RegionModel?> GetByRegionIdAsync(string regionId);
        Task<PaginatedResultModel<RegionModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<RegionModel> CreateAsync(RegionModel region);
        Task<RegionModel> UpdateAsync(RegionModel region);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteByRegionIdAsync(string regionId);
        Task<bool> ExistsByRegionIdAsync(string regionId);
    }

    public class RegionRepository : IRegionRepository
    {
        private readonly BaseDbContext _context;

        public RegionRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<RegionModel?> GetByIdAsync(Guid id)
        {
            return await _context.Regions.FindAsync(id);
        }

        public async Task<RegionModel?> GetByRegionIdAsync(string regionId)
        {
            return await _context.Regions
                .FirstOrDefaultAsync(r => r.RegionID == regionId);
        }

        public async Task<PaginatedResultModel<RegionModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Regions.AsQueryable();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<RegionModel> CreateAsync(RegionModel region)
        {
            region.Id = Guid.NewGuid();
            region.CreatedAt = DateTime.UtcNow;

            _context.Regions.Add(region);
            await _context.SaveChangesAsync(); 

            return region;
        }

        public async Task<RegionModel> UpdateAsync(RegionModel region)
        {
            region.UpdatedAt = DateTime.UtcNow;

            _context.Regions.Update(region);
            await _context.SaveChangesAsync();

            return region;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return false;
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByRegionIdAsync(string regionId)
        {
            var region = await _context.Regions
                .FirstOrDefaultAsync(r => r.RegionID == regionId);
            if (region == null)
            {
                return false;
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsByRegionIdAsync(string regionId)
        {
            return await _context.Regions
                .AnyAsync(r => r.RegionID == regionId);
        }
    }
}