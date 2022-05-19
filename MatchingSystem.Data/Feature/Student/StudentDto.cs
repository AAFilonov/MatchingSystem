namespace MatchingSystem.Data.Feature.Student;
#nullable enable
public class StudentDto
{
    public int? StudentID { get; set; }
    public int? GroupID { get; set; }
    public string GroupName { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }
    public string Patronimic { get; set; }
    public string Info { get; set; }
    public string Info2 { get; set; }
    public string WorkDirectionsName_List { get; set; }
    public string TechnologiesName_List { get; set; }
}