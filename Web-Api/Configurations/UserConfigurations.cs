using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web_Api.Model;

namespace Web_Api.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .HasOne(u => u.Team)  // Каждый User связан с одной Team
                .WithMany(t => t.Users)  // Одна Team может содержать несколько User
                .HasForeignKey(u => u.IdTeam);  // Внешний ключ в таблице User
        }
    }
}
