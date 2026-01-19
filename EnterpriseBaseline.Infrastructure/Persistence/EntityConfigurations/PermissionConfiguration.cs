using EnterpriseBaseline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseBaseline.Infrastructure.Persistence.EntityConfigurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
