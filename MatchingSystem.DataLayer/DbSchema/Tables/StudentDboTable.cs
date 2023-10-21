using MatchingSystem.DataLayer.Feature.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.Tables;

public class StudentDboTable: IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> entity)
    {
        entity.Property(e => e.StudentId).HasColumnName("StudentID");
        entity.Property(e => e.GroupId).HasColumnName("GroupID");
        entity.Property(e => e.Info2).HasMaxLength(250);
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.StudentBk).HasColumnName("StudentBK");
        entity.HasKey(e => e.StudentId).HasName("PK_Students");
        entity.HasOne(d => d.Group)
            .WithMany(p => p.Students)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Students_Groups");
    }
}