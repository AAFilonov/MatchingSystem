using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Document;

public class DocumentRepository : Repository<Model.Document>, IDocumentRepository
{
    public DocumentRepository(DbContext context) : base(context)
    {
    }
}