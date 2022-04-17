using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.User;

public class UserTable : IEntityTypeConfiguration<Model.User>
{
    public void Configure(EntityTypeBuilder<Model.User> builder)
    {
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.Email).HasMaxLength(50);
        builder.Property(e => e.LastVisitDate).HasColumnType("datetime");
        builder.Property(e => e.Login).HasMaxLength(50);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Patronimic).HasMaxLength(100);
        builder.Property(e => e.Surname).HasMaxLength(100);
        builder.Property(e => e.UserBk).HasColumnName("UserBK");
        builder.Property(e => e.NameAbbreviation). HasComputedColumnSql("[Name] + ' ' + [Patronimic]+ ' ' + [Surname]");
    }
}