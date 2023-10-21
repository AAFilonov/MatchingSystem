using MatchingSystem.DataLayer.Feature.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.View;

public class UserFullInfoDboView : IEntityTypeConfiguration<UsersFullInfoView>
{
    public void Configure(EntityTypeBuilder<UsersFullInfoView> entity)
    {
        entity.HasNoKey();

        entity.ToView("Users_FullInfo", "dbo_v");

        entity.Property(e => e.GroupId).HasColumnName("GroupID");

        entity.Property(e => e.GroupName).HasMaxLength(100);

        entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

        entity.Property(e => e.Name).HasMaxLength(100);

        entity.Property(e => e.NameAbbreviation).HasMaxLength(106);

        entity.Property(e => e.Patronimic).HasMaxLength(100);

        entity.Property(e => e.RoleId).HasColumnName("RoleID");

        entity.Property(e => e.RoleName).HasMaxLength(50);

        entity.Property(e => e.RoleNameRu)
            .HasMaxLength(50)
            .HasColumnName("RoleName_ru");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.Surname).HasMaxLength(100);

        entity.Property(e => e.TutorId).HasColumnName("TutorID");

        entity.Property(e => e.UserId).HasColumnName("UserID");
    }
}