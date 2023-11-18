using MatchingSystem.DataLayer.Feature.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.View;

public class ProjectDboView: IEntityTypeConfiguration<ProjectView>
{
    public void Configure(EntityTypeBuilder<ProjectView> entity)
    {
        entity.HasNoKey();
        entity.ToView("Projects", "dbo_v");
        entity.Property(e => e.AvailableGroupsNameList).HasColumnName("AvailableGroupsName_List");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        entity.Property(e => e.ProjectName).HasMaxLength(200);

        entity.Property(e => e.QtyDescription).HasMaxLength(50);

        entity.Property(e => e.TechnologiesNameList).HasColumnName("TechnologiesName_List");

        entity.Property(e => e.TutorId).HasColumnName("TutorID");

        entity.Property(e => e.TutorIsClosed).HasColumnName("Tutor_IsClosed");

        entity.Property(e => e.TutorName).HasMaxLength(100);

        entity.Property(e => e.TutorNameAbbreviation).HasMaxLength(106);

        entity.Property(e => e.TutorPatronimic).HasMaxLength(100);

        entity.Property(e => e.TutorSurname).HasMaxLength(100);

        entity.Property(e => e.UserId).HasColumnName("UserID");

        entity.Property(e => e.WorkDirectionsNameList).HasColumnName("WorkDirectionsName_List");
    }
}