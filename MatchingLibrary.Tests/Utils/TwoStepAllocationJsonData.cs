using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;

namespace MatchingLibrary.Tests.Utils;

public class TwoStepAllocationJsonData
{
    public List<ComplexToOneAllocated> students { get; set; }= new();
    
    public List<ComplexTwoStepAllocated> lecturers { get; set; }= new();
    
    public List<ComplexDependedAllocated> projects { get; set; }= new();
    
    public Dictionary<string, List<string>> lecturersToProjects { get; set; } = new();
    public Dictionary<string, List<string>> lecturersPreferencesToStudents { get; set; } = new();
    public Dictionary<string, List<string>> studentsPreferencesToProjects { get; set; } = new();
    
    public Dictionary<string, List<string>> projectAssignedToStudents { get; set; } = new();

    public ITwoStepAllocation ToAllocation()
    {
        ITwoStepAllocation allocation = new TwoStepAllocation();
        
        foreach ((string? lecturerName, List<string>? projectsNames) in lecturersToProjects)
        {
            var lecturer = lecturers.Find(allocated => allocated.name == lecturerName);
            foreach (string projectName in projectsNames)
            {
                var project = projects.Find(allocated => allocated.name == projectName);
                lecturer.GetProjects().Add(project);
                project.SetOwner(lecturer);
            }
           
        }
        
        foreach ((string? lecturerName, List<string>? studentsNames) in lecturersPreferencesToStudents)
        {
            var lecturer = lecturers.Find(allocated => allocated.name == lecturerName);
            foreach (string studentName in studentsNames)
            {
                var student = students.Find(allocated => allocated.name == studentName);
                lecturer.GetPreferences().Add(student);
            }
        }
        
        foreach ((string? studentName, List<string>? projectsNames) in studentsPreferencesToProjects)
        {
            var student = students.Find(allocated => allocated.name == studentName);
            foreach (string projectName in projectsNames)
            {
                var project = projects.Find(allocated => allocated.name == projectName);
                student.GetPreferences().Add(project);
            }
        }
        
        foreach ((string? projectName, List<string>? studentsNames) in projectAssignedToStudents)
        {
            var project = projects.Find(allocated => allocated.name == projectName);
          
            foreach (string studentName in studentsNames)
            {
                var student = students.Find(allocated => allocated.name == studentName);
                student.Assign(project);
                project.Assign(student);
            }
        }

        allocation.GetStudents().AddRange(students);
        allocation.GetLecturers().AddRange(lecturers);
        allocation.GetProjects().AddRange(projects);
        return allocation;
    }
}