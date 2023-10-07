using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Matching;

public class MatchingTypeDboTable : IEntityTypeConfiguration<MatchingType>
{
    public void Configure(EntityTypeBuilder<MatchingType> entity)
    {
        entity.ToTable("MatchingType");
        entity.Property(e => e.MatchingTypeId).HasColumnName("MatchingTypeID");
        entity.Property(e => e.MatchingTypeName).HasMaxLength(50);
        entity.Property(e => e.MatchingTypeCode).IsRequired();
        entity.Property(e => e.MatchingTypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("MatchingTypeName_ru");

        entity.HasKey(d => d.MatchingTypeId);
        entity.HasMany(d => d.Matchings)
            .WithOne(p => p.MatchingType)
            .HasForeignKey(d => d.MatchingTypeId)
            .HasConstraintName("FK_Matching_MatchingType");
    }
}