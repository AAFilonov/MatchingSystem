using TestStand.Allocated;

namespace MatchingLibrary.Algorithms.impl;

public class SpAlgorithm<S, L, P>
    where S : class, ISolitaryAllocated
    where L : class, ICapaciousAllocated
    where P : class, ICapaciousAllocated
{
    public TwoStepAllocation<S, L, P> allocate(TwoStepAllocation<S, L, P> allocation)
    {
        while (!isFinal(allocation))
        {
            computeIteration(allocation);
        }

        return allocation;
    }

    public void  computeIteration(TwoStepAllocation<S, L, P> allocation)
    {
        var freeStudents = allocation.students.FindAll(allocated => HaveReacheblePair(allocated, allocation));
        foreach (var student in freeStudents)
        {
            findPair(allocation, student);
        }
    }

    private void findPair(TwoStepAllocation<S, L, P> allocation, S student)
    {
        var studentPreferences = allocation.getStudentPreferences(student);
        if (studentPreferences.Count == 0)
            return;

        P project = studentPreferences.First();
        L lecturer = allocation.getLecturerByProject(project);
        
        var teacherPreferences = allocation.getLecturerPreferences(lecturer);
        //КОСТЫЛЬ - из за того что мы допускаем что предпочтения препода не являются полными
        //на множестве достижимых студентов,возможна ситуация когда препод (проект)
        //будет в предпочтениях студента, а студент в предпочтениях препода не будет 
        //в фактическом распределении студент назначается преподователю в список ожидания а после преподователь его сортирует
        //условие никогда не выполнится если бы предпочтения преподоавтеля были бы полными
        if (!teacherPreferences.Contains(student))      
        {
            allocation.deleteFromStudentPref(student,project);
            return; //cтудент не является примлемым для препода   
        }

        
        allocation.assign(student, project);
        var studentsByProject = allocation.getAssigned(project);
        var studentsByLecturer = allocation.getAssigned(lecturer);

        if (studentsByProject.Count > project.getCapacity())
        {
            S worstStudent = findWorstStudentByProject(allocation, project);
            allocation.breakAssigment(worstStudent, project);
        }

        if (studentsByLecturer.Count > lecturer.getCapacity())
        {
            S worstStudent = findWorstStudentByLecturer(allocation, lecturer);
            P projectWithWorstStudent = allocation.getProjectByAssigned(worstStudent);
            allocation.breakAssigment(worstStudent, projectWithWorstStudent);
        }

        if (studentsByProject.Count == project.getCapacity())
        {
            S worstStudent = findWorstStudentByProject(allocation, project);
            deleteSuccessors(allocation, worstStudent, project);
        }

        if (studentsByLecturer.Count == lecturer.getCapacity())
        {
            S worstStudent = findWorstStudentByLecturer(allocation, lecturer);
            deleteSuccessors(allocation, worstStudent, lecturer);
        }
    }

    private void deleteSuccessors(TwoStepAllocation<S, L, P> allocation, S student, L lecturer)
    {
        var preferences = allocation.getLecturerPreferences(lecturer);
        var lecturerProjects = allocation.getProjects(lecturer);
        int studentIndex = preferences.FindIndex(allocated => allocated == student);
        List<S> successors = preferences
            .GetRange(studentIndex + 1, preferences.Count - studentIndex - 1);
        successors.ForEach(successor =>
        {
          var desiredProjectsOfThisLecturer= allocation.getStudentPreferences(successor)
              .Where(project => lecturerProjects.Contains(project));

          foreach (var project in desiredProjectsOfThisLecturer)
          {
              allocation.deleteStudentPreferencePair(successor, project);  
          }
        });
    }

    private void deleteSuccessors(TwoStepAllocation<S, L, P> allocation, S student, P project)
    {
        L lecturer = allocation.getLecturerByProject(project);
        List<S> preferences = allocation.getLecturerPreferences(lecturer);
        
        var studentsReachableForThisProject = preferences
            .Where(allocated => allocation.getStudentPreferences(allocated).Contains(project)).ToList();
        int studentIndex = studentsReachableForThisProject.FindIndex(allocated => allocated == student);
        if (studentIndex == -1)
            return;
        var successors = studentsReachableForThisProject.GetRange(studentIndex + 1, studentsReachableForThisProject.Count - studentIndex - 1);
        successors.ForEach(successor => { allocation.deleteStudentPreferencePair(successor, project); });
    }

    private S findWorstStudentByLecturer(TwoStepAllocation<S, L, P> allocation, L lecturer)
    {
        List<S> preferences = new List<S>(allocation.getLecturerPreferences(lecturer));
        var assignedStudents = allocation.getAssigned(lecturer);

        bool AllNotAssigned(S allocated) => !assignedStudents.Contains(allocated);
        
        preferences.RemoveAll(AllNotAssigned);
        return preferences.Last();
    }

    private S findWorstStudentByProject(TwoStepAllocation<S, L, P> allocation, P project)
    {
        L lecturer = allocation.getLecturerByProject(project);
        List<S> preferences = new List<S>(allocation.getLecturerPreferences(lecturer));
        preferences.Reverse(); //sort in ascending order.


        var assignedStudents = allocation.getAssigned(project);
        S worstStudent = null;
        int worstScore = preferences.Count; //less is worse = хуже
        foreach (var student in assignedStudents)
        {
            var score = preferences.FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        };
        return worstStudent;
    }


    public bool isFinal(TwoStepAllocation<S, L, P> allocation)
    {
        var freeStudents = allocation.students.Where(allocated => HaveReacheblePair(allocated, allocation));
        return !freeStudents.Any();
    }

    bool HaveReacheblePair(S student, TwoStepAllocation<S, L, P> allocation)
    {
        bool havePair = allocation.getProjectByAssigned(student) != null;
        bool canFindPair = allocation.getStudentPreferences(student).Any();
        return !havePair && canFindPair;
    }
}