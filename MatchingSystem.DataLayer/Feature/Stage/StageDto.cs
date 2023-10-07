using System;
using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Feature.Stage
{
#nullable enable
    public class StageDto
    {
        public static StageDto fromEntity(StageEntity stage)
        {
            var dto = new StageDto
            {
                StageID = stage.StageId,
                StartDate = stage.StartDate,
                EndPlanDate = stage.EndPlanDate,
                IterationNumber = stage.IterationNumber,
                StageTypeCode = stage.StageType.StageTypeCode,
                StageTypeName_ru = stage.StageType.StageTypeNameRu
            };
            return dto;
        }

        [Key]
        public int StageID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndPlanDate { get; set; }
        public short? IterationNumber { get; set; }
        public int? StageTypeCode { get; set; } 
        public string? StageTypeName_ru { get; set; }
    }
}
