using Microsoft.EntityFrameworkCore;

namespace StandApi.Models
{
    public class StandContext : DbContext
    {
        public StandContext(DbContextOptions<StandContext> options)
            : base(options)
        {
        }

        public DbSet<Stand> Stands { get; set; }
        public DbSet<StandEntryDB> StandEntries { get; set; }
    }
}
