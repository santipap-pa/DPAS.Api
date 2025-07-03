
using DPAS.Api.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Context
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<AlertSettingModel> AlertSettings { get; set; }
        public DbSet<RegionModel> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionModel>()
                .OwnsOne(r => r.LocationCoordinates);
        }
    }
}