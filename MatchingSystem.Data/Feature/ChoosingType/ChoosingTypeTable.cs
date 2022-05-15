using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Matching;

public class ChoosingTypeTable : IEntityTypeConfiguration<Model.ChoosingType>
{
    public void Configure(EntityTypeBuilder<Model.ChoosingType> builder)
    {
        builder.HasKey(e => e.TypeId);

        builder.Property(e => e.TypeId).HasColumnName("TypeID");

        builder.Property(e => e.TypeName).HasMaxLength(50);

        builder.Property(e => e.TypeNameRu)
            .HasMaxLength(50)
            .HasColumnName("TypeName_ru");
    }
}