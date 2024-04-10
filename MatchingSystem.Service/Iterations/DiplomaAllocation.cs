using System.Collections.Generic;
using System.Linq;
using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using Newtonsoft.Json;

namespace MatchingSystem.Service.Iterations;

public class DiplomaAllocation : ITwoStepAllocation
{
    [JsonProperty] 
    public List<AllocatedTutor> lecturers = new();
    [JsonProperty] 
    public List<AllocatedStudent> students = new();
    [JsonProperty] 
    public List<AllocatedProject> projects = new();

    public List<IToOneAllocated> GetStudents()
    {
        return students.Cast<IToOneAllocated>().ToList();
    }

    public List<ITwoStepAllocated> GetLecturers()
    {
        return lecturers.Cast<ITwoStepAllocated>().ToList();
    }

    public List<IDependentAllocated> GetProjects()
    {
        return projects.Cast<IDependentAllocated>().ToList();
    }
}