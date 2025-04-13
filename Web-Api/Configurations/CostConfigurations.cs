using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web_Api.Model;

namespace Web_Api.Configurations
{
    public class CostConfigurations : IEntityTypeConfiguration<Cost>
    {
        public void Configure(EntityTypeBuilder<Cost> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(c => c.User)  // Каждый Cost связан с одним User
                .WithMany(u => u.Costs)  // Один User может иметь несколько Cost
                .HasForeignKey(c => c.IdUser);  // Внешний ключ в таблице Cost
        }
    }
}
