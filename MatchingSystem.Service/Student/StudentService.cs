using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.Service.Student;

public class StudentService : IStudentService
{
    private readonly IStudentRepository studentRepository;
    private readonly IProjectRepository projectRepository;

    public StudentService(
        IStudentRepository studentRepository,
        IProjectRepository projectRepository)
    {
        this.studentRepository = studentRepository;
        this.projectRepository = projectRepository;
    }


    public GetData GetSelectedParams(int studentId)
    {
            var student = studentRepository.GetStudent(studentId);
            var model = new GetData()
            {
                Technologies = studentRepository.GetTechnologiesSelectedByStudent(studentId),
                WorkDirections = studentRepository.GetWorkDirectionsSelectedByStudent(studentId),
                Info = student.Info,
                Info2 = student.Info2
            };
            return model;
    }

    public void EditProfile(EditProfileParams editParams)
    {
        studentRepository.EditProfile(editParams);
    }

    public IEnumerable<ProjectForStudent> GetProjects(int studentId)
    {
        var model = projectRepository.GetProjectsByStudent(studentId);

        return model.OrderBy(x => x.OrderNumber);
    }

    public void SetPreferences(int studentId,string selectedList)
    {
        var temp = selectedList.Split(',');
        var selectedIds = new int[temp.Length];

        
        studentRepository.ClearPreferences(studentId);

        for (var i = 0; i < temp.Length; i++)
        {
            selectedIds[i] = Convert.ToInt32(temp[i]);
            studentRepository.SetPreferences(new StudentPreferenceParams()
            {
                StudentId = studentId,
                Order = i + 1,
                SelectedProjectId = selectedIds[i]
            });
        }
    }

    public MatchingSystem.DataLayer.Entities.Student GetStudentInfo(int? studentId)
    {
        var student = studentRepository.GetStudent(studentId.Value);

        return student;
    }

    public AllocatedByStudent GetAllocatedProject(int? studentId)
    {
        var model = studentRepository.GetAllocationByStudent(studentId.Value);

        return model;
    }
}