using System.Linq.Expressions;
using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Algorithms.impl;

public class HrAlgorithm<S, T>
    where S : class, ISolitaryAllocated
    where T : class, ICapaciousAllocated

{
    public void computeStep(OneToManyAllocation<S, T> allocation)
    {
        var freeStudents = allocation.students.FindAll(allocated => HaveReacheblePair(allocated, allocation));
        foreach (var student in freeStudents)
        {
            findPair(allocation, student);
        }
    }

    private void findPair(OneToManyAllocation<S, T> allocation, S freeStudent)
    {
        var studetnPreferences = allocation.getPreferencesForStudent(freeStudent);

        if (!studetnPreferences.Any())
            return; //список предпочтений пуст

        var desiredTeacher = studetnPreferences.First();
        var teacherPreferences = allocation.getPreferencesForTeacher(desiredTeacher);
        if (!teacherPreferences.Contains(freeStudent))
        {
            deleteFromPreferences(allocation, desiredTeacher, freeStudent);
            return; //cтудент не является примлемым для препода   
        }


        allocation.setPair(desiredTeacher, freeStudent);
        var teatherAssigned = allocation.pairs[desiredTeacher];
        if (teatherAssigned.Count > desiredTeacher.getCapacity())
        {
            S worstAssignedStudent = findWorstAssignedStudent(desiredTeacher, allocation);
            deletePair(allocation, desiredTeacher, worstAssignedStudent);
        }
        else if (teatherAssigned.Count == desiredTeacher.getCapacity())
        {
            S worstAssignedStudent = findWorstAssignedStudent(desiredTeacher, allocation);
            int worstAssignedStudentIndex =
                teacherPreferences.FindIndex(allocated => allocated == worstAssignedStudent);
            teacherPreferences.RemoveRange(worstAssignedStudentIndex+1,
                teacherPreferences.Count - worstAssignedStudentIndex - 1);
        }
    }

    private static void deletePair(OneToManyAllocation<S, T> allocation, T desiredTeacher, S student)
    {
        allocation.pairs[desiredTeacher].Remove(student);
    }

    private static void deleteFromPreferences(OneToManyAllocation<S, T> allocation, T desiredTeacher, S student)
    {
        var prefs = allocation.getPreferencesForTeacher(desiredTeacher);
        prefs.Remove(student);
        allocation.teachersPreference[desiredTeacher] = prefs;
    }

    private S findWorstAssignedStudent(T teacher, OneToManyAllocation<S, T> allocation)
    {
        List<S> preferences = new List<S>(allocation.getPreferencesForTeacher(teacher));
        preferences.Reverse(); //список по возрастанию

        S worstStudent = null;
        int worstScore = preferences.Count; //меньше = хуже
        allocation.pairs[teacher].ForEach(student =>
        {
            var score = preferences.FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        });
        return worstStudent;
    }


    public bool isFinal(OneToManyAllocation<S, T> allocation)
    {
        var freeMen = allocation.students.Where(allocated => HaveReacheblePair(allocated, allocation));
        return !freeMen.Any();
    }

    bool HaveReacheblePair(S student, OneToManyAllocation<S, T> allocation)
    {
        bool havePair = allocation.pairs.Any(pair => pair.Value.Contains(student));
        bool canFindPair = allocation.getPreferencesForStudent(student).Any();
        return !havePair && canFindPair;
    }
}