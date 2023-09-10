using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.UserRole;

public class UserRoleTable : IEntityTypeConfiguration<UsersRole>
{
    public void Configure(EntityTypeBuilder<UsersRole> builder)
    {
        builder.HasKey(e => e.UserRoleId);
        builder.ToTable("Users_Roles");
        builder.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
        builder.Property(e => e.LastVisitDate).HasColumnType("datetime");
        builder.Property(e => e.MatchingId).HasColumnName("MatchingID");
        builder.Property(e => e.RoleId).HasColumnName("RoleID");
        builder.Property(e => e.StudentId).HasColumnName("StudentID");
        builder.Property(e => e.TutorId).HasColumnName("TutorID");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.HasOne(d => d.Matching)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.MatchingId)
            .HasConstraintName("FK_Users_Roles_Matching");
        builder.HasOne(d => d.Role)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Users_Roles_Roles");
        builder.HasOne(d => d.Student)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.StudentId)
            .HasConstraintName("FK_Users_Roles_Students");
        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.TutorId)
            .HasConstraintName("FK_Users_Roles_Tutors");
        builder.HasOne(d => d.User)
            .WithMany(p => p.UsersRoles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Users_Roles_Users");
    }
}