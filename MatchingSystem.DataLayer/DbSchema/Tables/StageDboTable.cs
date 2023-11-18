using MatchingSystem.DataLayer.Feature.Stage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.Tables;

public class StageDboTable: IEntityTypeConfiguration<StageEntity>
{
    public void Configure(EntityTypeBuilder<StageEntity> entity)
    {
        entity.Property(e => e.StageId).HasColumnName("StageID");
        entity.Property(e => e.EndDate).HasColumnType("datetime");
        entity.Property(e => e.EndPlanDate).HasColumnType("datetime");
        entity.Property(e => e.IsCurrent)
            .IsRequired()
            .HasDefaultValueSql("((1))");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.StageName).HasMaxLength(100);
        entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");
        entity.Property(e => e.StartDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
        
        entity.HasKey(e => e.StageId).HasName("PK_StagesIterations");
        
        entity.HasOne(d => d.Matching)
            .WithMany(p => p.Stages)
            .HasForeignKey(d => d.MatchingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Stages_Matching");

        entity.HasOne(d => d.StageType)
            .WithMany(p => p.Stages)
            .HasForeignKey(d => d.StageTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Stages_StagesTypes");
    }
}