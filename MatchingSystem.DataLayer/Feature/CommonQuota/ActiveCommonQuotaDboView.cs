using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class ActiveCommonQuotaDboView: IEntityTypeConfiguration<ActiveCommonQuotaView>
{
    public void Configure(EntityTypeBuilder<ActiveCommonQuotaView> entity)
    {
        entity.HasNoKey();

        entity.ToView("ActiveCommonQuotas", "dbo_v");

        entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");

        entity.Property(e => e.CreateDate).HasColumnType("datetime");

        entity.Property(e => e.Message).HasMaxLength(250);

        entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");

        entity.Property(e => e.StageId).HasColumnName("StageID");

        entity.Property(e => e.TutorId).HasColumnName("TutorID");

        entity.Property(e => e.UpdateDate).HasColumnType("datetime");
    }
}