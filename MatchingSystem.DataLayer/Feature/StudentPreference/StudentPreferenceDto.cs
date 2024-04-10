using System;

namespace MatchingSystem.DataLayer.Feature.StudentPreference
{
    
    public class StudentPreferenceDto
    {
        public StudentPreferenceDto()
        {
            
        }

       public StudentPreferenceDto(StudentsPreferenceEntity entity)
        {
            PreferenceId = entity.PreferenceId;
            StudentID = entity.StudentId;
            ProjectID = entity.ProjectId;
            OrderNumber = entity.OrderNumber;
            IsAvailable = entity.IsAvailable;
            TypeId = entity.TypeId;
            IsInUse = entity.IsInUse;
            IsUsed = entity.IsUsed;
            CreateDate = entity.CreateDate;
        }
        
        public int PreferenceId { get; set; }
        public int StudentID { get; set; }
        public int ProjectID { get; set; }
        public short OrderNumber { get; set; }
        public bool? IsAvailable { get; set; }
        public int? TypeId { get; set; }
        public bool? IsInUse { get; set; }
        public bool? IsUsed { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
