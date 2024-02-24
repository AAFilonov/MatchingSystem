namespace MatchingSystem.DataLayer.Feature.Stage;

public enum StageTypeEnum
{
    MATCHING_INITIALIZATION=1,
    MATCHING_PREPARATION=2,
    ACCUMULATION_STUDENTS_PREFERENCES=3,
    ITERATION=4,
    MANUAL_ADJUSTMENT=6,
    FINAL=7, 
    //DPL-39 выпилить этап ARCHIVE из базы 
}