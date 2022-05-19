using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Features;

public class UserRolesTable : IEntityTypeConfiguration<UsersRole>
{
    public void Configure(EntityTypeBuilder<UsersRole> entity)
    {
        entity.HasKey(e => e.UserRoleId);

        entity.ToTable("Users_Roles");

        entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

        entity.Property(e => e.LastVisitDate).HasColumnType("datetime");

        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

        entity.Property(e => e.RoleId).HasColumnName("RoleID");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.TutorId).HasColumnName("TutorID");

        entity.Property(e => e.UserId).HasColumnName("UserID");

        entity.HasOne(d => d.Matching)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.MatchingId)
            .HasConstraintName("FK_Users_Roles_Matching");

        entity.HasOne(d => d.Role)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Users_Roles_Roles");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.StudentId)
            .HasConstraintName("FK_Users_Roles_Students");

        entity.HasOne(d => d.Tutor)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.TutorId)
            .HasConstraintName("FK_Users_Roles_Tutors");

        entity.HasOne(d => d.User)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Users_Roles_Users");
    }
}
