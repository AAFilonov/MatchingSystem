using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.DataLayer.Feature.Tutor;

public class TutorDboTable: IEntityTypeConfiguration<TutorEntity>
{
    public void Configure(EntityTypeBuilder<TutorEntity> entity)
    {
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.TutorBk).HasColumnName("TutorBK");
        entity.HasKey(e => e.TutorId).HasName("PK_Tutors");
    }
}