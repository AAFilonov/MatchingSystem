using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto;

public class MatchingInitData
{
    public MatchingDto matching { get; set; } = new();
    public List<StudentDto> studentRecords { get; set; } = new();
    public List<TutorDto> tutorRecords { get; set; } = new();
    public List<GroupTutorDto> groupRecords { get; set; } = new();
}