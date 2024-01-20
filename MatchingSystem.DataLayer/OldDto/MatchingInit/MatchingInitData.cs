using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.OldDto.MatchingInit;

public class MatchingInitData
{
    public  int creatorId { get; set; }
    public MatchingInitDto matching { get; set; } = new();
    public List<StudentInitDto> studentRecords { get; set; } = new();
    public List<TutorInitDto> tutorRecords { get; set; } = new();
    public List<GroupInitDto> groupRecords { get; set; } = new();
}