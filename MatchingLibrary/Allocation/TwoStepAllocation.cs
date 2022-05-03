using TestStand.Allocated;

namespace MatchingLibrary;

public class TwoStepAllocation<S, L, P>
    where S : class, ISolitaryAllocated
    where L : class, ICapaciousAllocated
    where P : class, ICapaciousAllocated
{
    public TwoStepAllocation(List<S> students, List<L> lecturers)
    {
        this.students = students;
        this.lecturers = lecturers;
        //students.ForEach(allocated => studentsPreference.Add(allocated, new List<P>()));
        lecturers.ForEach(allocated => lecturersPreference.Add(allocated, new List<S>()));
    }

    //наблюдение - преференсы преподователе это упорядочивание множества всех студентов, считающих их /их проект примелемым        
    public List<S> students { get; set; }
    public List<L> lecturers { get; set; }


    private List<(S, P)> studentsPreferencePairs { get; set; } = new();
    private Dictionary<L, List<S>> lecturersPreference { get; set; } = new();


    private List<(L, P)> lecturerProjectPairs { get; set; } = new();

    private Dictionary<P, List<S>> waitingList { get; set; } = new();

    private List<(P, S)> waitingPairs { get; set; } = new();

    public void setProjects(L teacher, List<P> projects)
    {
        projects.ForEach(project => setProject(teacher, project));
    }

    public void setProject(L teacher, P project)
    {
        lecturerProjectPairs.Add((teacher, project));
    }

    public L getLecturerByProject(P project)
    {
        //TODO медленное решение
        return lecturerProjectPairs.FirstOrDefault(pair => pair.Item2 == project).Item1;
    }

    public void assign(S student, P project)
    {
        waitingPairs.Add((project, student));
    }

    public void breakAssigment(S student, P project)
    {
        waitingPairs.Remove((project, student));
    }

    public List<P> getStudentPreferences(S stundent)
    {
        if (students.Contains(stundent))
            return studentsPreferencePairs
                .FindAll(tuple => tuple.Item1 == stundent)
                .Select(tuple => tuple.Item2).ToList();
        else
            throw new ArgumentException($"Аргумент {stundent} не входит в список распределяемых!");
    }

    public List<S> getLecturerPreferences(L u)
    {
        if (lecturers.Contains(u))
            return lecturersPreference[u];
        else
            throw new ArgumentException($"Аргумент {u} не входит в список распределяемых!");
    }

    public void setStudentPreferences(S student, List<P> pref)
    {
        foreach (var project in pref)
            setStudentPreferencePair(student, project);
    }

    public void setStudentPreferencePair(S student, P project)
    {
        var projects = lecturerProjectPairs.Select(tuple => tuple.Item2).ToList();
        if (!projects.Contains(project))
            throw new ArgumentException($"Аргумент {project} не входит в список распределяемых {projects}!");

        if (!students.Contains(student))
            throw new ArgumentException($"Аргумент {student} не входит в список распределяемых {students}!");

        studentsPreferencePairs.Add((student, project));
    }

    public void deleteStudentPreferencePair(S student, P project)
    {
        studentsPreferencePairs.Remove((student, project));
    }

    public void setLecturerPreferences(L teacher, List<S> pref)
    {
        if (!pref.TrueForAll(allocated => students.Contains(allocated)))
        {
            throw new ArgumentException($"Аргументы из списка {pref} не входит в список распределяемых {students}!");
        }

        if (!lecturers.Contains(teacher))
            throw new ArgumentException($"Аргумент {teacher} не входит в список распределяемых {lecturers}!");

        lecturersPreference[teacher] = pref;
    }

    public void deleteFromStudentPref(S student, P project)
    {
        var preferences = getStudentPreferences(student);
        preferences.Remove(project); //лучше не удалять а сделать неактивными
        setStudentPreferences(student, preferences);
    }

    //TODO исключить этот метод
    public List<(L, List<(P, List<S>)>)> calcFinalAllocation()
    {
        var result = new List<(L, List<(P, List<S>)>)>();
        foreach (var lecturer in lecturers)
        {
            List<(P, List<S>)> projectStudentPairTuples = new List<(P, List<S>)>();
            var projects = getProjects(lecturer);
            projects.ForEach(project =>
            {
                var assignedStudents = getAssigned(project);
                projectStudentPairTuples.Add((project, assignedStudents));
            });
            result.Add(new(lecturer, projectStudentPairTuples));
        }

        return result;
    }

    public List<P> getProjects(L lecturer)
    {
        return lecturerProjectPairs
            .Where(tuple => tuple.Item1 == lecturer)
            .Select(tuple => tuple.Item2).ToList();
    }

    public List<S> getAssigned(P project)
    {
        return waitingPairs
            .FindAll(pair => pair.Item1 == project)
            .Select(pair => pair.Item2).ToList();
    }

    public List<S> getAssigned(L lecturer)
    {
        List<P> projects = lecturerProjectPairs
            .FindAll(pair => pair.Item1 == lecturer)
            .Select(tuple => tuple.Item2)
            .ToList();

        return waitingPairs
            .FindAll(pair => projects.Contains(pair.Item1))
            .Select(pair => pair.Item2).ToList();
    }

    public P? getProjectByAssigned(S s)
    {
        var pairs = waitingPairs.Where(pair => pair.Item2 == s).ToList();
        if (pairs.Count <= 0)
            return null;
        else
            return pairs.First().Item1;
    }
}