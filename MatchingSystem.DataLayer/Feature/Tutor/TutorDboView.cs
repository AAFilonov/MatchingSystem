using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Tutor;

public class TutorDboView: IEntityTypeConfiguration<TutorView>
{
    public void Configure(EntityTypeBuilder<TutorView> entity)
    {
        entity.HasNoKey();
        entity.ToView("Tutors", "dbo_v");
        entity.Property(e => e.LastVisitDate).HasColumnType("datetime");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.NameAbbreviation).HasMaxLength(106);
        entity.Property(e => e.Patronimic).HasMaxLength(100);
        entity.Property(e => e.Surname).HasMaxLength(100);
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.UserId).HasColumnName("UserID");
    }
}