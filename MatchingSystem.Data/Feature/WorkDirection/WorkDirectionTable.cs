using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Features;

public class WorkDirectionTable : IEntityTypeConfiguration<WorkDirection>
{
    public void Configure(EntityTypeBuilder<WorkDirection> entity)
    {
        entity.HasKey(e => e.DirectionId)
                .HasName("PK_Directions");

        entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

        entity.Property(e => e.DirectionNameRu)
            .HasMaxLength(200)
            .HasColumnName("DirectionName_ru");
    }
}