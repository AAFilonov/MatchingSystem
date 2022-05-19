using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Features;

public class RoleTable : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.Property(e => e.RoleId).HasColumnName("RoleID");

        entity.Property(e => e.RoleName).HasMaxLength(50);

        entity.Property(e => e.RoleNameRu)
            .HasMaxLength(50)
            .HasColumnName("RoleName_ru");
    }
}
