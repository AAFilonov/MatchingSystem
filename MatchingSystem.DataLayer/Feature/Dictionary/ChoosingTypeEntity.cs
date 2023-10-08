using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Dictionary;

public class ChoosingTypeEntity
{
    public ChoosingTypeEntity()
    {
        StudentsPreferences = new HashSet<StudentsPreference>();
        TutorsChoices = new HashSet<TutorsChoiceEntity>();
    }

    public int TypeId { get; set; }
    public int TypeCode { get; set; }
    public string TypeName { get; set; } = null!;
    public string? TypeNameRu { get; set; }

    public virtual ICollection<StudentsPreference> StudentsPreferences { get; set; }
    public virtual ICollection<TutorsChoiceEntity> TutorsChoices { get; set; }
}