using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web_Api.Model;

namespace Web_Api.Configurations
{
    public class TeamConfigurations : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasMany(t => t.Users)  // Одна команда может содержать несколько пользователей
                .WithOne(u => u.Team);
        }
    }
}


