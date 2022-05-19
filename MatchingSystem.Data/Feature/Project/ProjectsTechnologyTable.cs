using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Project;

public class ProjectsTechnologyTable : IEntityTypeConfiguration<ProjectsTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectsTechnology> entity)
    {
        entity.HasKey(e => e.ProjectTechnologyId)
                .HasName("PK_ProjectsTechnologies");

        entity.ToTable("Projects_Technologies");

        entity.Property(e => e.ProjectTechnologyId).HasColumnName("ProjectTechnologyID");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

        entity.HasOne(d => d.Project)
            .WithMany(p => p.ProjectsTechnologies)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Technologies_Projects");

        entity.HasOne(d => d.Technology)
            .WithMany(p => p.ProjectsTechnologies)
            .HasForeignKey(d => d.TechnologyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Technologies_Technologies");
    }
}