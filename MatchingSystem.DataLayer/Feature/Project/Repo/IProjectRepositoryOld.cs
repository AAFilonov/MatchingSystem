using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.IO.Params;

namespace MatchingSystem.DataLayer.Feature.Project.Repo
{
    public interface IProjectRepositoryOld
    {
   
        IEnumerable<TutorInitDto> CreateDefaultProjectsForTutors(List<TutorInitDto> tutors, int matchingId);
        void SetDefaultProjects_Groups(List<TutorInitDto> tuts);
        void CreateProject(ProjectParams @params);

        void EditProject(ProjectParams @params);
  
        void DeleteProject(int projectId);
    
        void UpdateProjectQuotaStage3(int tutorId, int projectId, int newQty);
      
        void UpdateProjectQuotaStage4(int tutorId, int projectId, short projectQuota);
     
        void SetProjectClose(int tutorId, int projectId);

        IEnumerable<ProjectDto> GetProjectsByTutor(int tutorId);
        public IEnumerable<ProjectDto> GetProjectsByMatching(int MatchingId);
        IEnumerable<ProjectForStudentDto> GetProjectsByStudent(int studentId);
    }
}
