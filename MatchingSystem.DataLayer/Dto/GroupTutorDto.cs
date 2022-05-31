namespace MatchingSystem.DataLayer.Dto;

public class GroupTutorDto
{
    private string name { get; set; }
    private bool value { get; set; }
    public GroupTutorDto(string name, bool value)
    {
        this.name = name;
        this.value = value;
    }
        
}