using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Student;

public class StudentsDboView: IEntityTypeConfiguration<StudentView>
{
    public void Configure(EntityTypeBuilder<StudentView> entity)
    {
        entity.HasNoKey();
        entity.ToView("Students", "dbo_v");
        entity.Property(e => e.GroupId).HasColumnName("GroupID");
        entity.Property(e => e.GroupName).HasMaxLength(100);
        entity.Property(e => e.Info2).HasMaxLength(250);
        entity.Property(e => e.LastVisitDate).HasColumnType("datetime");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.NameAbbreviation).HasMaxLength(106);
        entity.Property(e => e.Patronimic).HasMaxLength(100);
        entity.Property(e => e.StudentId).HasColumnName("StudentID");
        entity.Property(e => e.Surname).HasMaxLength(100);
        entity.Property(e => e.TechnologiesNameList).HasColumnName("TechnologiesName_List");
        entity.Property(e => e.UserId).HasColumnName("UserID");
        entity.Property(e => e.WorkDirectionsNameList).HasColumnName("WorkDirectionsName_List");
    }
}