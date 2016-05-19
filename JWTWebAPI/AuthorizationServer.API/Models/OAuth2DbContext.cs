using AuthorizationServer.API.Migrations;
using System.Data.Entity;

namespace AuthorizationServer.API.Models
{
    public class OAuth2DbContext : DbContext
    {
        public OAuth2DbContext() : base("OAuth2") 
        {
            Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<OAuth2DbContext, Configuration>());
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Audience> Audience { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}