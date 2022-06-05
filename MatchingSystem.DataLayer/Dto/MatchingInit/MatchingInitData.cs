using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingInit;

public class MatchingInitData
{
    public MatchingInitDto matching { get; set; } = new();
    public List<StudentInitDto> studentRecords { get; set; } = new();
    public List<TutorDtoInit> tutorRecords { get; set; } = new();
    public List<GroupInitDto> groupRecords { get; set; } = new();
}