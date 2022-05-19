using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Stage;
public class StagesTypeTable : IEntityTypeConfiguration<StagesType>
{
    public void Configure(EntityTypeBuilder<StagesType> entity)
    {
        entity.HasKey(e => e.StageTypeId);

        entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

        entity.Property(e => e.StageTypeName).HasMaxLength(50);

        entity.Property(e => e.StageTypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("StageTypeName_ru");
    }
}
