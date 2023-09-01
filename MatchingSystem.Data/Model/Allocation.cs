namespace MatchingSystem.DataLayer.Model;

public class Allocation
{
    public int MatchingId { get; set; }
    public int StageTypeId { get; set; }
    public int StageTypeCode { get; set; }
    public string StageTypeName { get; set; } = null!;
    public string? StageTypeNameRu { get; set; }
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public int? ChoiceId { get; set; }
    public int? ProjectId { get; set; }
    public short? SortOrderNumber { get; set; }
    public int? PreferenceId { get; set; }
    public int IsAllocated { get; set; }
    public int? TypeId { get; set; }
    public int? TypeCode { get; set; }
    public string? TypeName { get; set; }
    public string? TypeNameRu { get; set; }
}