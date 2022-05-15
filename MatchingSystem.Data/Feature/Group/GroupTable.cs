using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Group;

public class GroupTable : IEntityTypeConfiguration<Model.Group>
{
    public void Configure(EntityTypeBuilder<Model.Group> builder)
    {
        builder.Property(e => e.GroupId).HasColumnName("GroupID");

        builder.Property(e => e.GroupBk).HasColumnName("GroupBK");

        builder.Property(e => e.GroupName).HasMaxLength(100);

        builder.Property(e => e.MatchingId).HasColumnName("MatchingID");

        builder.HasOne(d => d.Matching)
            .WithMany(p => p.Groups)
            .HasForeignKey(d => d.MatchingId)
            .HasConstraintName("FK_Groups_Matching");
    }
}