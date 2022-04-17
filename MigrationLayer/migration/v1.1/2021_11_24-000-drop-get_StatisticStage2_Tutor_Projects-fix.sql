USE [DiplomaMatching]
GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage2_Tutor_Projects]    Script Date: 24.11.2021 18:42:57 ******/
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
    ProjectID						as ProjectID
     ,ProjectName					as ProjectName
     ,TechnologiesName_List			as ProjectTechnologiesName_List
     ,WorkDirectionsName_List		as ProjectWorkDirectionsName_List
     ,Qty							as ProjectsQty
     ,AvailableGroupsName_List		as ProjectAvailableGroupsName_List
from
    dbo_v.Projects with(nolock)
WHERE
    Projects.MatchingID = @MatchingID
  AND
    Projects.TutorID = @TutorID

