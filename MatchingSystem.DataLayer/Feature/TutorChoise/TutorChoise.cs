using System;
using System.Linq;

namespace MatchingSystem.DataLayer.Feature.TutorChoise;
#nullable enable
public class TutorChoiceDto
{
    public TutorChoiceDto(TutorsChoiceEntity entity)
    {
        ChoiceID = entity.ChoiceId;
        StudentID = entity.StudentId;
        StudentNameAbbreviation = entity.Student.UsersRoles.First().User.NameAbbreviation;
        GroupName = entity.Student.Group.GroupName;
        ProjectID = entity.ProjectId;
        ProjectName = entity.Project.ProjectName;
        IsInQuota = entity.IsInQuota;
        Qty = entity.Project.ProjectQuotaQty;
        SortOrderNumber = entity.SortOrderNumber;
        ProjectIsClosed = entity.Project.IsClosed;
        UpdateDate = entity.UpdateDate;
        TypeCode = entity.typeEntity.TypeCode;
    }

    public int? ChoiceID { get; set; }
    public int? StudentID { get; set; }
    public string? StudentNameAbbreviation { get; set; }
    public string? GroupName { get; set; }
    public int? ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public bool? IsInQuota { get; set; }
    public short? Qty { get; set; }
    public short? SortOrderNumber { get; set; }
    public bool? ProjectIsClosed { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? TypeCode { get; set; }
}