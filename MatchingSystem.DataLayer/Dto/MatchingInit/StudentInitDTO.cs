namespace MatchingSystem.DataLayer.Dto.MatchingInit;

public class StudentInitDto
{
    public StudentInitDto(string groupName, string firstName, string middleName, string lastName, string password)
    {
        this.groupName = groupName;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.password = password;
    }

    public StudentInitDto()
    {
    }

    public string groupName { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string lastName { get; set; }
    public string password { get; set; }
    public int UserId { get; set; }
    public int StudentId { get; set; }
    public int GroupId { get; set; }
}

