using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Student;
public class StudentsPreferenceTable : IEntityTypeConfiguration<StudentsPreference>
{
    public void Configure(EntityTypeBuilder<StudentsPreference> entity)
    {
        entity.HasKey(e => e.PreferenceId);

        entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

        entity.Property(e => e.CreateDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        entity.Property(e => e.IsAvailable)
            .IsRequired()
            .HasDefaultValueSql("((1))");

        entity.Property(e => e.IsInUse).HasDefaultValueSql("((0))");

        entity.Property(e => e.IsUsed).HasDefaultValueSql("((0))");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.TypeId)
            .HasColumnName("TypeID")
            .HasDefaultValueSql("((1))");

        entity.HasOne(d => d.Project)
            .WithMany(p => p.StudentsPreferences)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_StudentsPreferences_Projects");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.StudentsPreferences)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_StudentsPreferences_Students");

        entity.HasOne(d => d.Type)
            .WithMany(p => p.StudentsPreferences)
            .HasForeignKey(d => d.TypeId)
            .HasConstraintName("FK_StudentsPreferences_ChoosingTypes");
    }
}

