using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Matching;

public class MatchingDboTable : IEntityTypeConfiguration<MatchingEntity>
{
    public void Configure(EntityTypeBuilder<MatchingEntity> builder)
    {
        builder.ToTable("Matching");
        builder.Property(e => e.MatchingId).HasColumnName("MatchingID");
        builder.Property(e => e.CreatorUserId).HasColumnName("CreatorUserID");
        builder.Property(e => e.MatchingName).HasMaxLength(100);
        builder.Property(e => e.MatchingTypeId).HasColumnName("MatchingTypeID");


        builder.HasKey(e => e.MatchingId).HasName("PK_Matching");
        builder.HasOne(d => d.MatchingType)
            .WithMany(p => p.Matchings)
            .HasForeignKey(d => d.MatchingTypeId)
            .HasConstraintName("FK_Matching_MatchingType");
    }
}