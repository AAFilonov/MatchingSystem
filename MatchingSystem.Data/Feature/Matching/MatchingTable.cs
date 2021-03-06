using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Matching;

public class MatchingTable : IEntityTypeConfiguration<Model.Matching>
{
    public void Configure(EntityTypeBuilder<Model.Matching> builder)
    {
        builder.ToTable("Matching");
        builder.Property(e => e.MatchingId).HasColumnName("MatchingID");
        builder.Property(e => e.CreatorUserId).HasColumnName("CreatorUserID");
        builder.Property(e => e.MatchingName).HasMaxLength(100);

        builder.Property(e => e.MatchingTypeId).HasColumnName("MatchingTypeID");

        builder.HasOne(d => d.MatchingType)
            .WithMany(p => p.Matchings)
            .HasForeignKey(d => d.MatchingTypeId)
            .HasConstraintName("FK_Matching_MatchingType");
    }
}