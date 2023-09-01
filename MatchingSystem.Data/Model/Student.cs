using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Model;

public class Student
{
    public Student()
    {
        StudentsPreferences = new HashSet<StudentsPreference>();
        StudentsTechnologies = new HashSet<StudentsTechnology>();
        StudentsWorkDirections = new HashSet<StudentsWorkDirection>();
        TutorsChoices = new HashSet<TutorsChoice>();
        UsersRoles = new HashSet<UsersRole>();
    }

    public int StudentId { get; set; }
    public int? StudentBk { get; set; }
    public int GroupId { get; set; }
    public string? Info { get; set; }
    public int? MatchingId { get; set; }
    public string? Info2 { get; set; }

    public virtual Group Group { get; set; } = null!;
    public virtual ICollection<StudentsPreference> StudentsPreferences { get; set; }
    public virtual ICollection<StudentsTechnology> StudentsTechnologies { get; set; }
    public virtual ICollection<StudentsWorkDirection> StudentsWorkDirections { get; set; }
    public virtual ICollection<TutorsChoice> TutorsChoices { get; set; }
    public virtual ICollection<UsersRole> UsersRoles { get; set; }
}