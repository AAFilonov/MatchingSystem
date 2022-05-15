using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class ChoosingType
{
    public ChoosingType()
    {
        StudentsPreferences = new HashSet<StudentsPreference>();
        TutorsChoices = new HashSet<TutorsChoice>();
    }

    public int TypeId { get; set; }
    public int TypeCode { get; set; }
    public string TypeName { get; set; } = null!;
    public string? TypeNameRu { get; set; }

    public virtual ICollection<StudentsPreference> StudentsPreferences { get; set; }
    public virtual ICollection<TutorsChoice> TutorsChoices { get; set; }
}