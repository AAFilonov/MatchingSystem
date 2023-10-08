using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Allocation;

public class AllocationDboView : IEntityTypeConfiguration<AllocationView>
{
    public void Configure(EntityTypeBuilder<AllocationView> entity)
    {
        entity.HasNoKey();

        entity.ToView("Allocation", "dbo_v");

        entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");

        entity.Property(e => e.GroupId).HasColumnName("GroupID");

        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");

        entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.Property(e => e.StageTypeId).HasColumnName("StageTypeID");

        entity.Property(e => e.StageTypeName).HasMaxLength(50);

        entity.Property(e => e.StageTypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("StageTypeName_ru");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.TypeId).HasColumnName("TypeID");

        entity.Property(e => e.TypeName).HasMaxLength(50);

        entity.Property(e => e.TypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("TypeName_ru");
    }
}