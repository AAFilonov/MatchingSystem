using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingSystem.UI.ResultModels;
using Microsoft.AspNetCore.Mvc;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using MatchingSystem.UI.Dto.Student;
namespace Service.Student
{
    internal class StudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly IProjectRepository projectRepository;

        public StudentService(IStudentRepository studentRepository, IProjectRepository projectRepository)
        {
            this.studentRepository = studentRepository;
            this.projectRepository = projectRepository;
        }


        public IActionResult GetSelectedParams(int studentId)
        {
                var student = studentRepository.GetStudent(studentId);
                var model = new GetData()
                {
                    Technologies = studentRepository.GetTechnologiesSelectedByStudent(studentId),
                    WorkDirections = studentRepository.GetWorkDirectionsSelectedByStudent(studentId),
                    Info = student.Info,
                    Info2 = student.Info2
                };

                return new JsonResult(model);
        }

        public void EditProfile(int studentId,string info,string info2,string tech,string workDirection)
        {
            studentRepository.EditProfile(new EditProfileParams()
            {
                StudentId = Convert.ToInt32(studentId),
                Info = info,
                Info2 = info2,
                TechnologyCodeList = tech,
                WorkDirectionCodeList = workDirection
            });
        }

        public IActionResult GetProjects([FromQuery] int studentId)
        {
            var model = projectRepository.GetProjectsByStudent(studentId);

            return new JsonResult(model.OrderBy(x => x.OrderNumber));
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

        public IActionResult GetStudentInfo([FromQuery] int? studentId)
        {
            var student = studentRepository.GetStudent(studentId.Value);

            return new JsonResult(student);
        }

        public IActionResult GetAllocatedProject([FromQuery] int? studentId)
        {
            var model = studentRepository.GetAllocationByStudent(studentId.Value);

            return new JsonResult(model);
        }
    }
}
