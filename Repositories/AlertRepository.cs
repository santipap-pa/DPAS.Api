using DPAS.Api.Context;
using DPAS.Api.Enums;
using DPAS.Api.Extensions;
using DPAS.Api.Models.Data;
using DPAS.Api.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Repositories
{
    public interface IAlertRepository
    {
        Task<AlertModel?> GetByIdAsync(Guid id);
        Task<PaginatedResultModel<AlertModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<AlertModel?> GetByRegionIdAndDisasterTypeAsync(Guid regionId, DisasterTypeEnum disasterType);
        Task<AlertModel> CreateAsync(AlertModel alert);
        Task<AlertModel> UpdateAsync(AlertModel alert);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> HasAlertsAsync(Guid regionId);
    }

    public class AlertRepository : IAlertRepository
    {
        private readonly BaseDbContext _context;

        public AlertRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<AlertModel?> GetByIdAsync(Guid id)
        {
            return await _context.Alerts
                .Include(a => a.Region)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedResultModel<AlertModel>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Alerts
                .Include(a => a.Region)
                .AsQueryable();

            return await query.ToPagedResultAsync(pageNumber, pageSize);
        }

        public async Task<AlertModel?> GetByRegionIdAndDisasterTypeAsync(Guid regionId ,DisasterTypeEnum disasterType)
        {
            var query = _context.Alerts
                .Include(a => a.Region)
                .Where(a => a.RegionId == regionId && a.DisasterType == disasterType)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }
       

        public async Task<AlertModel> CreateAsync(AlertModel alert)
        {
            alert.Id = Guid.NewGuid();
            alert.CreatedAt = DateTime.UtcNow;

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(alert.Id) ?? alert;
        }

        public async Task<AlertModel> UpdateAsync(AlertModel alert)
        {
            alert.UpdatedAt = DateTime.UtcNow;

            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync();

            return alert;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null)
            {
                return false;
            }

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Alerts
                .AnyAsync(a => a.Id == id);
        }

        public async Task<bool> HasAlertsAsync(Guid regionId)
        {
            return await _context.Alerts
                .AnyAsync(a => a.RegionId == regionId);
        }
    }
}