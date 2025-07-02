
using Microsoft.EntityFrameworkCore;

namespace DPAS.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // public DbSet<YourEntity> YourEntities { get; set; 
    }
}