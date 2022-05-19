using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class DocumentTable : IEntityTypeConfiguration<Document>
{


    public void Configure(EntityTypeBuilder<Document> entity)
    {
        entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
        entity.Property(e => e.DocumentName).HasMaxLength(100);
        entity.Property(e => e.StageId).HasColumnName("StageID");
        entity.HasOne(d => d.Stage)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.StageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Documents_Stages");
    }
}