using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Project;

public class ProjectDboTable: IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> entity)
    {
        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        entity.Property(e => e.CloseDate).HasColumnType("datetime");
        entity.Property(e => e.CreateDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.ProjectName).HasMaxLength(200);
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.UpdateDate).HasColumnType("datetime");

        entity.HasKey(d => d.ProjectId).HasName("PK_Projects");
        entity.HasOne(d => d.Tutor)
            .WithMany(p => p.Projects)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Tutors");
    }
}