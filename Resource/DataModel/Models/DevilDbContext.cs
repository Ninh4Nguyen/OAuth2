using DataModel.Migrations;
using System.Data.Entity;

namespace DataModel.Models
{
    public class DevilDbContext : DbContext
    {
        public DevilDbContext(): base("Devil")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DevilDbContext, Configuration>());
        }
        public DbSet<Product> Product { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
