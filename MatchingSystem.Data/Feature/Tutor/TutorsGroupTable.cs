using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.TutorsGroup;

public class TutorsGroupTable : IEntityTypeConfiguration<Model.TutorsGroup>
{
    public void Configure(EntityTypeBuilder<Model.TutorsGroup> entity)
    {
        entity.HasKey(e => e.TutorGroupId);

        entity.ToTable("Tutors_Groups");

        entity.Property(e => e.TutorGroupId).HasColumnName("TutorGroupID");

        entity.Property(e => e.GroupId).HasColumnName("GroupID");

        entity.Property(e => e.TutorId).HasColumnName("TutorID");

        entity.HasOne(d => d.Group)
            .WithMany(p => p.TutorsGroups)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Tutors_Groups_Groups");

        entity.HasOne(d => d.Tutor)
            .WithMany(p => p.TutorsGroups)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Tutors_Groups_Tutors");
    }
}
