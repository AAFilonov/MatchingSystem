using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Matching;

public class MatchingTypeTable : IEntityTypeConfiguration<MatchingType>
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
    }
}