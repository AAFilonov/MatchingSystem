using System;
using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingLibrary.Allocated;
using MatchingSystem.DataLayer.Feature.Project;
using Newtonsoft.Json;

namespace MatchingSystem.Service.Iterations;

[NoArgsConstructor]
public partial class AllocatedProject : IDependentAllocated
{
    [JsonIgnore]
    public readonly ProjectEntity _entity;

    [JsonIgnore]
    public List<AllocatedStudent> assignedStudents=new();

    [JsonIgnore]
    public AllocatedTutor tutor;

    public int projectId => _entity.ProjectId;
    public int capacity;

    public List<int> assignedStudentsIds_InDB =>
        _entity.TutorsChoices
            .DistinctBy(entity =>entity.StudentId)
            .Where(entity => entity.IsInQuota).Select(x => x.StudentId).ToList();
    public List<int> assignedStudentsIds_InDTO =>
        assignedStudents.Select(x => x.studentId).ToList();

    public AllocatedProject(ProjectEntity entity)
    {
        this._entity = entity;
        capacity = entity.ProjectQuotaQty ?? Int32.MaxValue;
    }

    object IAllocated.GetAssigned()
    {
        return GetAssigned();
    }

    public int GetCapacity()
    {
        return capacity;
    }

    public void SetCapacity(int newCapacity)
    {
        capacity = newCapacity;
    }

    public ITwoStepAllocated GetOwner()
    {
        return tutor;
    }

    public List<IAllocated> GetAssigned()
    {
        return assignedStudents.Cast<IAllocated>().ToList();
    }

    public void Assign(IAllocated assigned)
    {
        assignedStudents.Add(assigned as AllocatedStudent);
    }

    public void breakAssigment(IAllocated assigned)
    {
        assignedStudents.Remove(assigned as AllocatedStudent);
    }

    public List<IAllocated> GetPreferences()
    {
        throw new System.Exception(
            "В двухшаговом распределении нет прямого использования предпочтений проектов."
        );
    }

    public void SetPreferences(List<IAllocated> preferences)
    {
        throw new System.Exception(
            "В двухшаговом распределении нет прямого использования предпочтений проектов."
        );
    }

    public void InitAssigned(List<AllocatedStudent> allocationStudents)
    {
        assignedStudents = allocationStudents.Where(student => assignedStudentsIds_InDB.Contains(student.studentId)).ToList();
    }

    public void InitTutors(List<AllocatedTutor> allocationLecturers)
    {
        tutor = allocationLecturers.SingleOrDefault(tutor => _entity.Tutor.TutorId == tutor.tutorId);
    }
}