using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class QuotaRequest
    {
        [Key]
        public int QuotaID { get; set; }
        public int TutorID { get; set; }
        public string? NameAbbreviation { get; set; }
        public short RequestedQuotaQty { get; set; }
        public string? Message { get; set; }
        public int CurrentQuotaQty { get; set; }
    }
}
