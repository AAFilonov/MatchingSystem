namespace MatchingSystem.DataLayer.Dto.IO.Request
{
    public class AdjustmentRequest
    {
        public int MatchingId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public int StudentId { get; set; }
    }
}
