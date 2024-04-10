using System;
using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using Newtonsoft.Json;

namespace MatchingSystem.Service.Iterations;

[NoArgsConstructor]
public partial class AllocatedTutor : ITwoStepAllocated
{
    private readonly TutorEntity entity;

    [JsonIgnore]
    public List<AllocatedProject> projects = new();

    [JsonIgnore]
    public List<AllocatedStudent> preferences = new();

    public int tutorId => entity.TutorId;
    public int capacity;
    public List<int> ownedProjectIds => projects.Select(project => project.projectId).ToList();
    public List<int> preferenceStudentsIds => preferences.Select(student => student.studentId).ToList();
    
    public AllocatedTutor(TutorEntity entity)
    {
        this.entity = entity;
        capacity = this.entity.CommonQuota.Single(quotaEntity => quotaEntity.QuotaStateId == (int)CommonQuotaStateEnum.ACTIVE).Qty ?? Int32.MaxValue;
    }

    public void InitProjects(List<AllocatedProject> allocationProjects)
    {
        projects = new List<AllocatedProject>();
        projects.AddRange(allocationProjects.Where(project =>
                entity.Projects.Any(pref => pref.ProjectId == project.projectId)
            )
        );
    }
    
    public void InitPreferences(ITwoStepAllocation allocation)
    {
        List<TutorsChoiceEntity> choices = entity.Projects
            .SelectMany(projects => projects.TutorsChoices)
            .OrderBy(choiceEntity => choiceEntity.SortOrderNumber)
            .ToList();
        List<AllocatedStudent> acceptableStudents = allocation.GetStudents().Cast<AllocatedStudent>().Where(student =>
                choices.Any(pref => pref.StudentId == student.studentId)
            )
            .OrderBy(student => choices.FindIndex(choiceEntity => choiceEntity.StudentId == student.studentId)).ToList()
            .ToList();
        
        preferences.AddRange(acceptableStudents);
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

    public List<IDependentAllocated> GetProjects()
    {
        return projects.Cast<IDependentAllocated>().ToList();
    }

    public void SetProjects(List<IDependentAllocated> newProjects)
    {
        projects = newProjects.Cast<AllocatedProject>().ToList();
    }



    public List<IAllocated> GetAssigned()
    {
        return projects
            .SelectMany(project => project.GetAssigned())
            .ToList();
    }

    public void Assign(IAllocated assigned)
    {
        throw new System.Exception(
            "Для преподавателя в двухшаговом распределении приямое назначене студентов недосутпно"
        );
    }

    public void breakAssigment(IAllocated assigned)
    {
        throw new System.Exception(
            "Для преподавателя в двухшаговом распределении приямое назначене студентов недосутпно"
        );
    }

    public List<IAllocated> GetPreferences()
    {
        return preferences.Cast<IAllocated>().ToList();
    }

    public void SetPreferences(List<IAllocated> preferences)
    {
        this.preferences = preferences.Cast<AllocatedStudent>().ToList();
    }
}