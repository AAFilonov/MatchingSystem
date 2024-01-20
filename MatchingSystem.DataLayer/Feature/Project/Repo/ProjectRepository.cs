using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.Project.Repo;

public class ProjectRepository : GenericRepository<ProjectEntity>, IProjectRepository
{
    public List<TutorInitDto> CreateDefaultProjectsForTutors(List<TutorInitDto> tutors, int matchingId,
        DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context =>
        {
            var projects = tutors.Select(dto =>
            {
                return new ProjectEntity
                {
                    IsClosed = false,
                    IsDefault = true,
                    TutorId = dto.TutorId,
                    MatchingId = matchingId
                };
            }).ToList();
            saveRange(projects);
            for (var i = 0; i < tutors.Count; i++) tutors[i].DefaultProjectId = projects[i].ProjectId;
        }, extContext);
        return tutors;
    }

    public void AssignProjectsToGroups(List<TutorInitDto> tutors,  DiplomaMatchingContext? extContext = null)
    {
        СallInContext(context =>
        {
            var projectsGroupEntities = new List<ProjectsGroupEntity>();
            tutors.ForEach(tutor =>
            {
                tutor.groups.ForEach(group =>
                {
                    projectsGroupEntities.Add(new ProjectsGroupEntity
                    {
                        GroupId = group.groupId,
                        ProjectId = tutor.DefaultProjectId
                    });
                });
            });
            context.ProjectsGroups.AddRange(projectsGroupEntities);
        },extContext);
    }
}