using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Role;

public class RoleDboTable : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.Property(e => e.RoleId).HasColumnName("RoleID");

        builder.Property(e => e.RoleName).HasMaxLength(50);

        builder.Property(e => e.RoleNameRu)
            .HasMaxLength(50)
            .HasColumnName("RoleName_ru");
        builder.HasKey(e => e.RoleId).HasName("PK_Roles");
    }
}