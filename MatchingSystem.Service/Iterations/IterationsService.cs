using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingLibrary.Allocators;
using MatchingLibrary.Allocators.interfaces;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.TutorChoice.repo;

namespace MatchingSystem.Service.Iterations;

[AllArgsConstructor]
public partial class IterationsService : Repository, IIterationsService
{

    private readonly ITutorRepository tutorRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorsChoiceRepository tutorsChoiceRepository;
    private static readonly ITwoStepAllocatorSplited _allocator = new TwoStepAllocatorSplited();
    //сначала исходя из данных которые сохранены в БД мы формируем дто-объект allocation
    public DiplomaAllocation getAllocation(StageEntity currentStage)
    {
        return CallInContext(context =>
        {
            var allocation = new DiplomaAllocation();

            List<AllocatedProject> projects = projectRepository.findAll()
                .Where(entity => entity.MatchingId == currentStage.MatchingId)
                .Select(entity => new AllocatedProject(entity))
                .ToList();
            allocation.projects.AddRange(projects);
            
            List<AllocatedStudent> students = studentRepository.findAll()
                .Where(entity => entity.MatchingId == currentStage.MatchingId)
               // .Include(student => student.StudentsPreferences)
               // .Include(student => student.TutorsChoices)
                .Select(entity => new AllocatedStudent(entity))
                .ToList();
            allocation.students.AddRange(students);
            
            List<AllocatedTutor> tutors = tutorRepository.findAll()
                .Where(entity => entity.MatchingId == currentStage.MatchingId)
                .Select(entity => new AllocatedTutor(entity))
                .ToList();
            allocation.lecturers.AddRange(tutors);
  
            allocation.projects.ForEach(project => project.InitAssigned(allocation.students));
            allocation.projects.ForEach(project => project.InitTutors(allocation.lecturers));
            
            allocation.students.ForEach(student => student.InitPreferences(allocation.projects));
            allocation.students.ForEach(student => student.InitAssigned(allocation.projects));
            
            allocation.lecturers.ForEach(tutor => tutor.InitProjects(allocation.projects));
            allocation.lecturers.ForEach(tutor => tutor.InitPreferences(allocation));
            
            return allocation;
        });
    }
    //затем мы выполняем с ним манипуляции, после чего зеркалим измененный объект обратно в БД
    public void applyAllocation(DiplomaAllocation allocation)
    {
        //return CallInTransaction(context =>
        //{
        //    //allocation.projects.ForEach(project => project.Assign(allocation.students));
        //});
        
        //возможные события
        // студент был назначен на проект и там остался
        // студент был отвергнут преподом 
        // студент остался назначен где был назначен
        //студенту удалили часть записей из предпочтений - они больше недоступны
        
    }
    
    public bool isInFinalIteration(DiplomaAllocation allocation)
    {
        return _allocator.isFinal(allocation);
    }

    public void computeIteration(DiplomaAllocation allocation)
    {
        _allocator.computeIteration(allocation);
    }

    public void startIteration(DiplomaAllocation allocation)
    {
        _allocator.StartIteration(allocation);
    }

    public void finishIteration(DiplomaAllocation allocation)
    {
        _allocator.finishIteration(allocation);
    }

    public void generateTutorChoices(DiplomaAllocation allocation)
    {
        // сформировать начальные предпочтения в алфавитном поррядке из студентов которые назначены на текущей итерации
        //TODO DPL-4 generate tutorChoices
        _allocator.InitLecturerPreferences(allocation);
    }
}