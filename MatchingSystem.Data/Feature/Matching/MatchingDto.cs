namespace MatchingSystem.DataLayer.Feature.Matching;
#nullable enable
public class MatchingDto
{
    public int MatchingID { get; set; }
    public string MatchingName { get; set; }
    public int? MatchingTypeID { get; set; }
    public int? CreatorUserID { get; set; }

}