using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Project;

public class ProjectsWorkDirectionTable : IEntityTypeConfiguration<ProjectsWorkDirection>
{
    public void Configure(EntityTypeBuilder<ProjectsWorkDirection> entity)
    {
        entity.HasKey(e => e.ProjectDirectionId)
                .HasName("PK_Projects_Directions");

        entity.ToTable("Projects_WorkDirections");

        entity.Property(e => e.ProjectDirectionId).HasColumnName("ProjectDirectionID");

        entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.HasOne(d => d.Direction)
            .WithMany(p => p.ProjectsWorkDirections)
            .HasForeignKey(d => d.DirectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_WorkDirections_WorkDirections");

        entity.HasOne(d => d.Project)
            .WithMany(p => p.ProjectsWorkDirections)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_WorkDirections_Projects");
    }
}