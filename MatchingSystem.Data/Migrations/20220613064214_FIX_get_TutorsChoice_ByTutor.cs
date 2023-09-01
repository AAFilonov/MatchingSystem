#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace MatchingSystem.Data.Migrations
{
    public partial class FIX_get_TutorsChoice_ByTutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            --actual SQL here
USE [DiplomaMatching]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.03.2020
-- Update date: 25.04.2020
-- Description:	
-- =============================================
ALTER FUNCTION [napp].[get_TutorsChoice_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
	select 
		Choice.ChoiceID
		
		,Student.StudentID
		,Student.NameAbbreviation as StudentNameAbbreviation
		,Student.GroupName
	
		,Project.TutorID
	
		,Project.ProjectID
		,Project.ProjectName 
		,Project.IsClosed	as ProjectIsClosed
		,Project.Qty 
		,Project.QtyDescription
	
		,Choice.SortOrderNumber 
		,Choice.IsInQuota
		,Choice.IsChangeble
		,Choice.PreferenceID
		,Choice.UpdateDate
		,Choice.IsFromPreviousIteration

		,ChoosingTypes.TypeID
		,ChoosingTypes.TypeCode
		,ChoosingTypes.TypeName
		,ChoosingTypes.TypeName_ru
	
	
	from 
		dbo_v.Projects Project with (nolock)

		left join dbo.TutorsChoice Choice with (nolock) on 
			Choice.ProjectID = Project.ProjectID
			and 
			Choice.StageID = napp_in.get_CurrentStageID_ByMatching (Project.MatchingID) 	
		
		left join dbo_v.Students Student  with (nolock) on
			Student.StudentID = Choice.StudentID
	
		--join dbo_v.Projects Project with (nolock) on 
		--	Project.ProjectID = Choice.ProjectID	

		left join dbo.ChoosingTypes ChoosingTypes with (nolock) on 
			ChoosingTypes.TypeID = Choice.TypeID
	
	where 
		--Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID) 	
		Project.TutorID = @TutorID
		--and 
		--Choice.StageID = napp_in.get_CurrentStageID_ByMatching (Project.MatchingID) 	

            GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            --actual SQL here
USE [DiplomaMatching]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.03.2020
-- Update date: 25.04.2020
-- Description:	
-- =============================================
ALTER FUNCTION [napp].[get_TutorsChoice_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
	select 
		Choice.ChoiceID
		
		,Student.StudentID
		,Student.NameAbbreviation as StudentNameAbbreviation
		,Student.GroupName
	
		,Project.TutorID
	
		,Project.ProjectID
		,Project.ProjectName 
		,Project.IsClosed	as ProjectIsClosed
		,Project.Qty 
		,Project.QtyDescription
	
		,Choice.SortOrderNumber 
		,Choice.IsInQuota
		,Choice.IsChangeble
		,Choice.PreferenceID
		,Choice.IsFromPreviousIteration

		,ChoosingTypes.TypeID
		,ChoosingTypes.TypeCode
		,ChoosingTypes.TypeName
		,ChoosingTypes.TypeName_ru
	
	
	from 
		dbo_v.Projects Project with (nolock)

		left join dbo.TutorsChoice Choice with (nolock) on 
			Choice.ProjectID = Project.ProjectID
			and 
			Choice.StageID = napp_in.get_CurrentStageID_ByMatching (Project.MatchingID) 	
		
		left join dbo_v.Students Student  with (nolock) on
			Student.StudentID = Choice.StudentID
	
		--join dbo_v.Projects Project with (nolock) on 
		--	Project.ProjectID = Choice.ProjectID	

		left join dbo.ChoosingTypes ChoosingTypes with (nolock) on 
			ChoosingTypes.TypeID = Choice.TypeID
	
	where 
		--Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID) 	
		Project.TutorID = @TutorID
		--and 
		--Choice.StageID = napp_in.get_CurrentStageID_ByMatching (Project.MatchingID) 	

            GO");
        }
    }
}
