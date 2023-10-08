using MatchingSystem.DataLayer.Feature.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class CommonQuotaDboTable : IEntityTypeConfiguration<CommonQuotaEntity>
{
    public void Configure(EntityTypeBuilder<CommonQuotaEntity> entity)
    {
        entity.Property(e => e.CommonQuotaId).HasColumnName("CommonQuotaID");
        entity.Property(e => e.CreateDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
        entity.Property(e => e.Message).HasMaxLength(250);
        entity.Property(e => e.QuotaStateId).HasColumnName("QuotaStateID");
        entity.Property(e => e.StageId).HasColumnName("StageID");
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        
        entity.HasKey(e => e.CommonQuotaId).HasName("PK_CommonQuotas");
        
        entity.HasOne(d => d.QuotaState)
            .WithMany(p => p.CommonQuota)
            .HasForeignKey(d => d.QuotaStateId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CommonQuotas_QuotasStates");
        entity.HasOne(d => d.Stage)
            .WithMany(p => p.CommonQuota)
            .HasForeignKey(d => d.StageId)
            .HasConstraintName("FK_CommonQuotas_Stages");
        entity.HasOne(d => d.Tutor)
            .WithMany(p => p.CommonQuota)
            .HasForeignKey(d => d.TutorId)
            .HasConstraintName("FK_CommonQuotas_Tutors");
    }
}