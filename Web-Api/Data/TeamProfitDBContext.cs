using Microsoft.EntityFrameworkCore;
using Web_Api.Model;

namespace Web_Api.Data
{
    public class TeamProfitDBContext : DbContext
    {
        public TeamProfitDBContext(DbContextOptions<TeamProfitDBContext> options) 
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cost> Costs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Указание первичного ключа для модели User
            modelBuilder.Entity<User>()
                .HasKey(u => u.IdUser);

            // Указание первичного ключа для модели Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.IdProject);

            // Указание первичного ключа для модели User
            modelBuilder.Entity<Team>()
                .HasKey(u => u.IdTeam);

            // Указание первичного ключа для модели Project
            modelBuilder.Entity<Cost>()
                .HasKey(p => p.IdCosts);

            // Связь один ко многим: один User может иметь несколько Cost
            modelBuilder.Entity<Cost>()
                .HasOne(c => c.User)  // Каждый Cost связан с одним User
                .WithMany(u => u.Costs)  // Один User может иметь несколько Cost
                .HasForeignKey(c => c.IdUser);  // Внешний ключ в таблице Cost

            // Связь один ко многим: один User может принадлежать только одной Team
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)  // Каждый User связан с одной Team
                .WithMany(t => t.Users)  // Одна Team может содержать несколько User
                .HasForeignKey(u => u.IdTeam);  // Внешний ключ в таблице User

            // Связь один к одному: один Project связан с одной Team
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Team)  // Каждый Project связан с одной Team
                .WithOne()  // Одна Team может работать над одним Project
                .HasForeignKey<Project>(p => p.IdTeam);  // Внешний ключ в таблице Project
        }
    }
}
