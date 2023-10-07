using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.DataLayer.Model;

public class Document
{
    public int DocumentId { get; set; }
    public int StageId { get; set; }
    public string Path { get; set; } = null!;
    public string DocumentName { get; set; } = null!;

    public virtual StageEntity Stage { get; set; } = null!;
}