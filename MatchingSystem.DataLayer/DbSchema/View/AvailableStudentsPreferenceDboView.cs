using MatchingSystem.DataLayer.Feature.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.View;

public class AvailableStudentsPreferenceDboView: IEntityTypeConfiguration<AvailableStudentsPreferenceView>
{
    public void Configure(EntityTypeBuilder<AvailableStudentsPreferenceView> entity)
    {
        entity.HasNoKey();
        entity.ToView("AvailableStudentsPreferences", "dbo_v");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");
        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        entity.Property(e => e.StudentId).HasColumnName("StudentID");
    }
}