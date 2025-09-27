using Microsoft.EntityFrameworkCore;
using TheNexusAPI.Entities;

namespace TheNexusAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Individual> Individual => Set<Individual>();
        public DbSet<Family> Family => Set<Family>();

    }
}
