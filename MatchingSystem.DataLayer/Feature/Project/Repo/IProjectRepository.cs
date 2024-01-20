using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.Project.Repo;

public interface IProjectRepository:IRepository<ProjectEntity>
{
    List<TutorInitDto> CreateDefaultProjectsForTutors(List<TutorInitDto> tutors, int matchingId, DiplomaMatchingContext? extContext = null);
    void AssignProjectsToGroups(List<TutorInitDto> tutors, DiplomaMatchingContext? extContext = null);
}