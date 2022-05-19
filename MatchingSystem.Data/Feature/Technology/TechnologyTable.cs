using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Technology;

public class TechnologyTable : IEntityTypeConfiguration<Model.Technology>
{
    public void Configure(EntityTypeBuilder<Model.Technology> entity)
    {
        entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

        entity.Property(e => e.TechnologyNameRu)
            .HasMaxLength(200)
            .HasColumnName("TechnologyName_ru");
    }
}