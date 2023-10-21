using MatchingSystem.DataLayer.Feature.Allocation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.DbSchema.View;

public class AllocationFullInfoDboView : IEntityTypeConfiguration<AllocationFullInfoView>
{

    public void Configure(EntityTypeBuilder<AllocationFullInfoView> entity)
    {
        entity.HasNoKey();
        entity.ToView("Allocation_FullInfo", "dbo_v");
        entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");
        entity.Property(e => e.GroupId).HasColumnName("GroupID");
        entity.Property(e => e.GroupName).HasMaxLength(100);
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");
        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        entity.Property(e => e.ProjectName).HasMaxLength(200);
        entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");
        entity.Property(e => e.StageTypeName).HasMaxLength(50);
        entity.Property(e => e.StageTypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("StageTypeName_ru");
        entity.Property(e => e.StudentId).HasColumnName("StudentID");
        entity.Property(e => e.StudentName).HasMaxLength(100);
        entity.Property(e => e.StudentNameAbbreviation).HasMaxLength(106);
        entity.Property(e => e.StudentPatronimic).HasMaxLength(100);
        entity.Property(e => e.StudentSurname).HasMaxLength(100);
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.TutorName).HasMaxLength(100);
        entity.Property(e => e.TutorNameAbbreviation).HasMaxLength(106);
        entity.Property(e => e.TutorPatronimic).HasMaxLength(100);
        entity.Property(e => e.TutorSurname).HasMaxLength(100);
        entity.Property(e => e.TypeId).HasColumnName("TypeID");
        entity.Property(e => e.TypeName).HasMaxLength(50);
        entity.Property(e => e.TypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("TypeName_ru");
    }
}