using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Student;
public class StudentsWorkDirectionTable : IEntityTypeConfiguration<StudentsWorkDirection>
{
    public void Configure(EntityTypeBuilder<StudentsWorkDirection> entity)
    {
        entity.HasKey(e => e.StudentDirectionId)
                .HasName("PK_StudentsDirections");

        entity.ToTable("Students_WorkDirections");

        entity.Property(e => e.StudentDirectionId).HasColumnName("StudentDirectionID");

        entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.HasOne(d => d.Direction)
            .WithMany(p => p.StudentsWorkDirections)
            .HasForeignKey(d => d.DirectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_WorkDirections_WorkDirections");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.StudentsWorkDirections)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_WorkDirections_Students");
    }
}

