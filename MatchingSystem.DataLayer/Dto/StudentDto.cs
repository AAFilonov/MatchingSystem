using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;

public class StudentDto
{
    public StudentDto(string groupName, string firstName, string middleName, string lastName, string password)
    {
        this.groupName = groupName;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.password = password;
    }

    public StudentDto()
    {
    }

    public string groupName { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public string password { get; set; }
}
