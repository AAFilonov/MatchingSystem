using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Group;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.Feature.TutorChoice;

namespace MatchingSystem.DataLayer.Feature.Student;

public class StudentEntity
{
    public StudentEntity()
    {
        StudentsPreferences = new HashSet<StudentsPreferenceEntity>();
        StudentsTechnologies = new HashSet<StudentsTechnology>();
        StudentsWorkDirections = new HashSet<StudentsWorkDirection>();
        TutorsChoices = new HashSet<TutorsChoiceEntity>();
        UsersRoles = new HashSet<UsersRoleEntity>();
    }

    public int StudentId { get; set; }
    public int? StudentBk { get; set; }
    public int GroupId { get; set; }
    public string? Info { get; set; }
    public int? MatchingId { get; set; }
    public string? Info2 { get; set; }
    public bool IsDeleted { get; set; }

    public virtual GroupEntity Group { get; set; } = null!;
    public virtual ICollection<StudentsPreferenceEntity> StudentsPreferences { get; set; }
    public virtual ICollection<StudentsTechnology> StudentsTechnologies { get; set; }
    public virtual ICollection<StudentsWorkDirection> StudentsWorkDirections { get; set; }
    public virtual ICollection<TutorsChoiceEntity> TutorsChoices { get; set; }
    public virtual ICollection<UsersRoleEntity> UsersRoles { get; set; }
}