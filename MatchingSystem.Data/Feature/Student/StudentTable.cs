using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Featured;
public class StudentTable : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> entity)
    {
        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.GroupId).HasColumnName("GroupID");

        entity.Property(e => e.Info2).HasMaxLength(250);

        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

        entity.Property(e => e.StudentBk).HasColumnName("StudentBK");

        entity.HasOne(d => d.Group)
            .WithMany(p => p.Students)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_Groups");
    }
}

