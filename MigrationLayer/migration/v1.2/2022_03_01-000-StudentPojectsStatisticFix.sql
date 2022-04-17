USE [DiplomaMatching]
GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Student_Projects]    Script Date: 01.03.2022 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [napp].[get_StatisticStage3_Student_Projects]
(
@MatchingID int
,@StudentID int
)
RETURNS TABLE
as
return
SELECT
    StudentsPreferences.ProjectID				as StudentsPreferencesProjectID
     ,dbo_v.Projects.ProjectName					as ProjectsProjectName
     ,dbo_v.Projects.TechnologiesName_List		as ProjectsTechnologiesName_List
     ,dbo_v.Projects.WorkDirectionsName_List		as ProjectsWorkDirectionsName_List
     ,Qty										as ProjectQty
     ,dbo_v.Projects.AvailableGroupsName_List	as ProjectsAvailableGroupsName_List
     ,NameAbbreviation							as TutorNameAbbreviation
     ,Tutors.TutorID								as TutorID
     ,StudentsPreferences.OrderNumber			as OrderNumber
FROM
    StudentsPreferences with(nolock)
	JOIN Students ON StudentsPreferences.StudentID = Students.StudentID
    JOIN dbo_v.Projects ON Projects.ProjectID = StudentsPreferences.ProjectID
    JOIN dbo_v.Tutors on dbo_v.Tutors.TutorID = dbo_v.Projects.TutorID
WHERE Students.MatchingID = @MatchingID
  AND Students.StudentID = @StudentID	

