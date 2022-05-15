using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Tutor;

public class TutorTable : IEntityTypeConfiguration<Model.Tutor>
{
    public void Configure(EntityTypeBuilder<Model.Tutor> entity)
    {
        entity.Property(e => e.TutorId).HasColumnName("TutorID");
        entity.Property(e => e.MatchingId).HasColumnName("MatchingID");
        entity.Property(e => e.TutorBk).HasColumnName("TutorBK");
    }
}