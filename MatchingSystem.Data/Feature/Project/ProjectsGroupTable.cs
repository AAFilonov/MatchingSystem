using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Project;

public class ProjectsGroupTable : IEntityTypeConfiguration<ProjectsGroup>
{
    public void Configure(EntityTypeBuilder<ProjectsGroup> entity)
    {
        entity.HasKey(e => e.ProjectGroupId)
                .HasName("PK__Projects__17125E7ED62BC05D");

        entity.ToTable("Projects_Groups");

        entity.Property(e => e.ProjectGroupId).HasColumnName("ProjectGroupID");

        entity.Property(e => e.GroupId).HasColumnName("GroupID");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.HasOne(d => d.Group)
            .WithMany(p => p.ProjectsGroups)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Groups_Groups");

        entity.HasOne(d => d.Project)
            .WithMany(p => p.ProjectsGroups)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Projects_Groups_Projects");
    }
}