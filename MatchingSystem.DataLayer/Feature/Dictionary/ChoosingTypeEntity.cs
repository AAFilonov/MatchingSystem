using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.Feature.TutorChoise;

namespace MatchingSystem.DataLayer.Feature.Dictionary;

public class ChoosingTypeEntity
{
    public ChoosingTypeEntity()
    {
        StudentsPreferences = new HashSet<StudentsPreferenceEntity>();
        TutorsChoices = new HashSet<TutorsChoiceEntity>();
    }

    public int TypeId { get; set; }
    public int TypeCode { get; set; }
    public string TypeName { get; set; } = null!;
    public string? TypeNameRu { get; set; }

    public virtual ICollection<StudentsPreferenceEntity> StudentsPreferences { get; set; }
    public virtual ICollection<TutorsChoiceEntity> TutorsChoices { get; set; }
}