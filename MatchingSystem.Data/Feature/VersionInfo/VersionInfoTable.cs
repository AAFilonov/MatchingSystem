using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Features;

public class VersionInfoTable : IEntityTypeConfiguration<VersionInfo>
{
    public void Configure(EntityTypeBuilder<VersionInfo> entity)
    {
        entity.HasNoKey();

        entity.ToTable("VersionInfo");

        entity.HasIndex(e => e.Version, "UC_Version")
            .IsUnique()
            .IsClustered();

        entity.Property(e => e.AppliedOn).HasColumnType("datetime");
        entity.Property(e => e.Description).HasMaxLength(1024);
    }
}
