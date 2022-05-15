using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Student;
public class StudentsTechnologyTable : IEntityTypeConfiguration<StudentsTechnology>
{
    public void Configure(EntityTypeBuilder<StudentsTechnology> entity)
    {
        entity.HasKey(e => e.StudentTechnologyId)
                .HasName("PK_StudentsTechnologies");

        entity.ToTable("Students_Technologies");

        entity.Property(e => e.StudentTechnologyId).HasColumnName("StudentTechnologyID");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.StudentsTechnologies)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_Technologies_Students");

        entity.HasOne(d => d.Technology)
            .WithMany(p => p.StudentsTechnologies)
            .HasForeignKey(d => d.TechnologyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_Technologies_Technologies");
    }
}

