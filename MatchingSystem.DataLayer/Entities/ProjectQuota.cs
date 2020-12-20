namespace MatchingSystem.DataLayer.Entities
{
    public class ProjectQuota
    {
        public int ProjectID { get; set; }
        public short Quota { get; set; }
        public static readonly string DbTypeName = "dbo.ProjectQuota";
    }
}