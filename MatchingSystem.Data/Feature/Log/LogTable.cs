using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchingSystem.Data.Feature.Log;

public class LogTable : IEntityTypeConfiguration<Model.Log>
{
    public void Configure(EntityTypeBuilder<Model.Log> builder)
    {
        builder.HasNoKey();
        builder.ToTable("Log");
        builder.Property(e => e.Endpoint).HasColumnType("text");
        builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        builder.Property(e => e.Request).HasColumnType("text");
    }
}