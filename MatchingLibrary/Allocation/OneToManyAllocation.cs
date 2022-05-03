using TestStand.Allocated;

namespace MatchingLibrary;

public class OneToManyAllocation<S, T>
    where S : class, ISolitaryAllocated //Student
    where T : class, ICapaciousAllocated //Teacher
{
    public OneToManyAllocation(List<S> students, List<T> teachers)
    {
        this.students = students;
        this.teachers = teachers;
        students.ForEach(allocated => studentsPreference.Add(allocated, new List<T>()));
        teachers.ForEach(allocated => teachersPreference.Add(allocated, new List<S>()));
    }

    public List<S> students { get; set; }
    public List<T> teachers { get; set; }

    public Dictionary<S, List<T>> studentsPreference { get; set; } = new();
    public Dictionary<T, List<S>> teachersPreference { get; set; } = new();

    public Dictionary<T, List<S>> pairs { get; set; } = new();


    public void setPair(T teacher, S student)
    {
        if (!pairs.ContainsKey(teacher))
        {
            pairs[teacher] = new List<S>();
        }

        pairs[teacher].Add(student);
    }

    public List<T> getPreferencesForStudent(S t)
    {
        if (students.Contains(t))
            return studentsPreference[t];
        else
            throw new ArgumentException($"Аргумент {t} не входит в список распределяемых!");
    }

    public List<S> getPreferencesForTeacher(T u)
    {
        if (teachers.Contains(u))
            return teachersPreference[u];
        else
            throw new ArgumentException($"Аргумент {u} не входит в список распределяемых!");
    }

    public void setPreferencesForStudent(S student, List<T> pref)
    {
        if (!pref.TrueForAll(allocated => teachers.Contains(allocated)))
        {
            throw new ArgumentException($"Аргументы из списка {pref} не входит в список распределяемых {teachers}!");
        }

        if (!students.Contains(student))
            throw new ArgumentException($"Аргумент {student} не входит в список распределяемых {students}!");

        studentsPreference[student] = pref;
    }

    public void setPreferencesForTeacher(T teacher, List<S> pref)
    {
        if (!pref.TrueForAll(allocated => students.Contains(allocated)))
        {
            throw new ArgumentException($"Аргументы из списка {pref} не входит в список распределяемых {students}!");
        }

        if (!teachers.Contains(teacher))
            throw new ArgumentException($"Аргумент {teacher} не входит в список распределяемых {teachers}!");

        teachersPreference[teacher] = pref;
    }

    public void deleteFromStudentPref(S student, T teacher)
    {
        var preferences = getPreferencesForStudent(student);
        preferences.Remove(teacher); //лучше не удалять а сделать неактивными
        setPreferencesForStudent(student, preferences);
    }

    public List<(T, List<S>)> calcFinalAllocation()
    {
        var result = new List<(T, List<S>)>();
        foreach (var pair in this.pairs)
        {
            result.Add(new(pair.Key, pair.Value));
        }

        teachers.Where(teacher => getPair(teacher).Count == 0).ToList().ForEach(teacher =>
        {
            result.Add(new(teacher, null)!);
        });

        students.Where(student => !pairs.Any(pair => pair.Value.Contains(student))).ToList().ForEach(student =>
        {
            result.Add(new(null, new List<S>() { student }));
        });
        return result;
    }

    private List<S> getPair(T teacher)
    {
        try
        {
            return pairs[teacher];
        }
        catch (Exception e)
        {
            return new List<S>();
        }
    }
}