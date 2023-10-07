using MatchingSystem.DataLayer.Feature.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class CommonQuotaDboView : IEntityTypeConfiguration<CommonQuotaView>
{
    public void Configure(EntityTypeBuilder<CommonQuotaView> entity)
    {
        entity.HasNoKey();
        entity.ToView("CommonQuotas", "dbo_v");
        entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");
        entity.Property(e => e.CreateDate).HasColumnType("datetime");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.Message).HasMaxLength(250);
        entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");
        entity.Property(e => e.QuotaStateName).HasMaxLength(50);
        entity.Property(e => e.QuotaStateNameRu)
            .HasMaxLength(50)
            .HasColumnName("QuotaStateName_ru");
        entity.Property(e => e.StageId).HasColumnName("StageID");
        entity.Property(e => e.StageTypeName).HasMaxLength(50);
        entity.Property(e => e.StageTypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("StageTypeName_ru");
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.UpdateDate).HasColumnType("datetime");
    }
}
