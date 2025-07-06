
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

        public DbSet<RegionModel> Regions { get; set; }
        public DbSet<AlertModel> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionModel>()
                .OwnsOne(r => r.LocationCoordinates);

            modelBuilder.Entity<AlertModel>()
                .HasOne(a => a.Region)
                .WithMany()
                .HasForeignKey(a => a.RegionId)
                .HasPrincipalKey(r => r.Id);
        }
    }
}