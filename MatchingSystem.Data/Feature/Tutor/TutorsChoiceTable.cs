using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.TutorsChoice;

public class TutorsChoiceTable : IEntityTypeConfiguration<Model.TutorsChoice>
{
    public void Configure(EntityTypeBuilder<Model.TutorsChoice> entity)
    {
        entity.HasKey(e => e.ChoiceId)
                .HasName("PK_TutorsMatching");

        entity.ToTable("TutorsChoice");

        entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");

        entity.Property(e => e.CreateDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("(getdate())");

        entity.Property(e => e.IsChangeble).HasDefaultValueSql("((1))");

        entity.Property(e => e.PreferenceId).HasColumnName("PreferenceID");

        entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

        entity.Property(e => e.SortOrderNumber).HasDefaultValueSql("((32767))");

        entity.Property(e => e.StageId).HasColumnName("StageID");

        entity.Property(e => e.StudentId).HasColumnName("StudentID");

        entity.Property(e => e.TypeId)
            .HasColumnName("TypeID")
            .HasDefaultValueSql("((2))");

        entity.Property(e => e.UpdateDate).HasColumnType("datetime");

        entity.HasOne(d => d.Project)
            .WithMany(p => p.TutorsChoices)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TutorsChoice_Projects");

        entity.HasOne(d => d.Stage)
            .WithMany(p => p.TutorsChoices)
            .HasForeignKey(d => d.StageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TutorsChoice_Stages");

        entity.HasOne(d => d.Student)
            .WithMany(p => p.TutorsChoices)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TutorsChoice_Students");

        entity.HasOne(d => d.Type)
            .WithMany(p => p.TutorsChoices)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TutorsChoice_ChoosingTypes");
    }
}