using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Group;

public class GroupDboTable : IEntityTypeConfiguration<GroupEntity>
{
    public void Configure(EntityTypeBuilder<GroupEntity> entity)
    {
        entity.Property(e => e.GroupId).HasColumnName("GroupID");
        entity.Property(e => e.GroupBk).HasColumnName("GroupBK");
        entity.Property(e => e.GroupName).HasMaxLength(100);
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.HasKey(e => e.GroupId).HasName("PK_Groups");
        entity.HasOne(d => d.Matching)
            .WithMany(p => p.Groups)
            .HasForeignKey(d => d.MatchingId)
            .HasConstraintName("FK_Groups_Matching");
    }
}