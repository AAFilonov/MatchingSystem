namespace MatchingSystem.Data.Feature.User;
#nullable enable
public class UserDto
{
    public int UserID { get; set; }
    public string Login { get; set; }
    #nullable enable
    public DateTime? LastVisitDate { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronimic { get; set; }
    public string? NameAbbreviation { get; set; }
}
