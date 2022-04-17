USE [DiplomaMatching]
GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage2_Tutor_Projects]    Script Date: 12.12.2021 16:59:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [napp].[get_StatisticStage2_Tutor_Projects]
(
@MatchingID int
,@TutorID int
)
RETURNS TABLE
as
return
SELECT
    ProjectID
     ,ProjectName
     ,TechnologiesName_List
     ,Info
     ,WorkDirectionsName_List
     ,Qty
     ,AvailableGroupsName_List
from
    dbo_v.Projects with(nolock)
WHERE
    Projects.MatchingID = @MatchingID
  AND
    Projects.TutorID = @TutorID
