using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingLibrary.Allocated;
using MatchingSystem.DataLayer.Feature.Student;
using Newtonsoft.Json;

namespace MatchingSystem.Service.Iterations;

[NoArgsConstructor]
public partial class AllocatedStudent : IToOneAllocated
{
    [JsonIgnore]
    public readonly StudentEntity _entity;

    [JsonIgnore]
    public List<AllocatedProject> preferences=new();

    [JsonIgnore]
    public AllocatedProject? assigned;

    public int studentId => _entity.StudentId;
    public int? assignedProjectId_InDB => _entity.TutorsChoices
        .OrderByDescending(entity =>entity.ChoiceId )
        .Take(1)
        .SingleOrDefault(choice => choice.IsInQuota)?.ProjectId;
    public int? assignedProjectId_InDto => assigned?.projectId;

    public List<int> preferenceProjectIds => preferences.Select(project => project.projectId).ToList();

    public AllocatedStudent(StudentEntity entity)
    {
        this._entity = entity;
    }

    public void InitPreferences(List<AllocatedProject> getProjects)
    {
        preferences = new List<AllocatedProject>();
        List<int> acceptableProjectsIds = _entity.StudentsPreferences
            .OrderBy(entity => entity.OrderNumber)
            .Select(entity => entity.ProjectId).ToList();

        List<AllocatedProject> acceptableProjects = getProjects.Where(project =>
            _entity.StudentsPreferences.Any(pref =>
                pref.ProjectId == project.projectId &&
                project._entity.IsClosed == false &&
                pref.IsUsed == false
            )
        ).ToList();

        var sortedProjects = acceptableProjects.OrderBy(project => acceptableProjectsIds.FindIndex(id => id == project.projectId)).ToList();
        preferences.AddRange(sortedProjects);
    }
    
    public void InitAssigned(List<AllocatedProject> allocationProjects)
    {
        //TutorsChoiceEntity assignedProject = _entity.TutorsChoices.Single(choice => choice.IsInQuota);
        assigned = allocationProjects.SingleOrDefault(project => project.projectId == assignedProjectId_InDB);
    }
    
    object IAllocated.GetAssigned()
    {
        return GetAssigned();
    }

    public IAllocated GetAssigned()
    {
        return assigned;
    }

    public void Assign(IAllocated assigned)
    {
        this.assigned = assigned as AllocatedProject;
    }

    public void breakAssigment(IAllocated assigned)
    {
        this.assigned = null;
        //update Tutororchoice
    }

    public List<IAllocated> GetPreferences()
    {
        return preferences.Cast<IAllocated>().ToList();
    }

    public void SetPreferences(List<IAllocated> preferences)
    {
        this.preferences = preferences.Cast<AllocatedProject>().ToList();
    }


}