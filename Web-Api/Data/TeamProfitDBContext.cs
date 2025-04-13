using Microsoft.EntityFrameworkCore;
using Web_Api.Configurations;
using Web_Api.Model;

namespace Web_Api.Data
{
    public class TeamProfitDBContext : DbContext
    {
        public TeamProfitDBContext(DbContextOptions<TeamProfitDBContext> options) : base(options) { }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cost> Costs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new CostConfigurations());

            base.OnModelCreating(modelBuilder);
        }
    }
}
