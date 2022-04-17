using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class AllocationFullInfo
{
    public int MatchingId { get; set; }
    public int StageTypeId { get; set; }
    public int StageTypeCode { get; set; }
    public string StageTypeName { get; set; } = null!;
    public string? StageTypeNameRu { get; set; }
    public int? StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentSurname { get; set; }
    public string? StudentPatronimic { get; set; }
    public string? StudentNameAbbreviation { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public int IsAllocated { get; set; }
    public int? ChoiceId { get; set; }
    public int? PreferenceId { get; set; }
    public short? SortOrderNumber { get; set; }
    public int? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public int? TutorId { get; set; }
    public string? TutorName { get; set; }
    public string? TutorSurname { get; set; }
    public string? TutorPatronimic { get; set; }
    public string? TutorNameAbbreviation { get; set; }
    public int? TypeId { get; set; }
    public int? TypeCode { get; set; }
    public string? TypeName { get; set; }
    public string? TypeNameRu { get; set; }
}