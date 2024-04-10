using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.Service.Iterations;

namespace MatchingSystem.Service.Tests.Utils;

public class AllocationDamp
{
    public DiplomaAllocation allocation;
    public List<StageDto> stages;
    public List<TutorChoiceDto> choisesHistory;
    public List<TutorChoiceDto> finalChoises;
    public List<StudentPreferenceDto> preferences;
}