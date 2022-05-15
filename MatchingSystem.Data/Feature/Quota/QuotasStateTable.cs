using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Features;

public class QuotasStateTable : IEntityTypeConfiguration<QuotasState>
{
    public void Configure(EntityTypeBuilder<QuotasState> entity)
    {
        entity.HasKey(e => e.QuotaStateId);

        entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

        entity.Property(e => e.QuotaStateName).HasMaxLength(50);

        entity.Property(e => e.QuotaStateNameRu)
            .HasMaxLength(50)
            .HasColumnName("QuotaStateName_ru");
    }
}
