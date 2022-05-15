using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Project;

public class ProjectTable : IEntityTypeConfiguration<Model.Project>
{
    public void Configure(EntityTypeBuilder<Model.Project> builder)
    {
        builder.Property(e => e.ProjectId).HasColumnName("ProjectID");

        builder.Property(e => e.CloseDate).HasColumnType("datetime");

        builder.Property(e => e.CreateDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.MatchingId).HasColumnName("MatchingID");

        builder.Property(e => e.ProjectName).HasMaxLength(200);

        builder.Property(e => e.TutorId).HasColumnName("TutorID");

        builder.Property(e => e.UpdateDate).HasColumnType("datetime");

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.Projects)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Tutors");
    }
}