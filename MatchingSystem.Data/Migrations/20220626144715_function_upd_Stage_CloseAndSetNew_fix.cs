using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingSystem.Data.Migrations
{
    public partial class function_upd_Stage_CloseAndSetNew_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
USE [DiplomaMatching]
GO
/****** Object:  StoredProcedure [napp_in].[upd_Stage_CloseAndSetNew]    Script Date: 25.06.2022 22:56:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 19.03.2020
-- Update date: 04.04.2020
-- Description:	Закрывает текущую итерацию и создает следующую. 
-- =============================================
ALTER PROCEDURE [napp_in].[upd_Stage_CloseAndSetNew]
	@CurDate datetime = null 
	,@CurStageID int
	,@CurStageTypeCode int
	,@NewIterationNumber int = null 
	,@MatchingID int
AS
BEGIN	

	declare @NewStageTypeID int; 

	begin tran ;

		update dbo.Stages 
		set 
			EndDate = @CurDate 
			,IsCurrent = 0
		where 
			StageID = @CurStageID; 
		
		if (@CurStageTypeCode = 1) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 2
									)
			; 
			set @NewIterationNumber = null; 
		end

		if (@CurStageTypeCode = 2) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 3 
									)
			; 
			set @NewIterationNumber = null; 
		end
		else if 
				(
					(@CurStageTypeCode = 3)
					or
					((@CurStageTypeCode = 4) and (@NewIterationNumber is not null))
				) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 4 
									)
			; 
			--set @NewIterationNumber = 1; 
		end
		else if ((@CurStageTypeCode = 4) and (@NewIterationNumber is null))
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 5 
									)
			; 
			--set @NewIterationNumber = 1; 
		end;
		if (@CurStageTypeCode = 5) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 6 
									)
			; 
			set @NewIterationNumber = null; 
		end
	

		insert into dbo.Stages 
		(
			StageTypeID 
			,StageName
			,IterationNumber
			,StartDate
			,EndPlanDate
			,IsCurrent
			,MatchingID
		) 
		select 
			@NewStageTypeID
			,null
			,@NewIterationNumber
			,@CurDate
			,null
			,1
			,@MatchingID
		;

	commit tran ; 

	return;

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
USE [DiplomaMatching]
GO
/****** Object:  StoredProcedure [napp_in].[upd_Stage_CloseAndSetNew]    Script Date: 26.06.2022 17:49:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 19.03.2020
-- Update date: 04.04.2020
-- Description:	Закрывает текущую итерацию и создает следующую. 
-- =============================================
ALTER PROCEDURE [napp_in].[upd_Stage_CloseAndSetNew]
	@CurDate datetime = null 
	,@CurStageID int
	,@CurStageTypeCode int
	,@NewIterationNumber int = null 
	,@MatchingID int
AS
BEGIN	

	declare @NewStageTypeID int; 

	begin tran ;

		update dbo.Stages 
		set 
			EndDate = @CurDate 
			,IsCurrent = 0
		where 
			StageID = @CurStageID; 


		if (@CurStageTypeCode = 2) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 3 
									)
			; 
			set @NewIterationNumber = null; 
		end
		else if 
				(
					(@CurStageTypeCode = 3)
					or
					((@CurStageTypeCode = 4) and (@NewIterationNumber is not null))
				) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 4 
									)
			; 
			--set @NewIterationNumber = 1; 
		end
		else if ((@CurStageTypeCode = 4) and (@NewIterationNumber is null))
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 5 
									)
			; 
			--set @NewIterationNumber = 1; 
		end;
		if (@CurStageTypeCode = 5) 
		begin
			set @NewStageTypeID = (	select top(1) 
										StageTypeID 
									from 
										dbo.StagesTypes with (nolock) 
									where 
										StageTypeCode = 6 
									)
			; 
			set @NewIterationNumber = null; 
		end
	

		insert into dbo.Stages 
		(
			StageTypeID 
			,StageName
			,IterationNumber
			,StartDate
			,EndPlanDate
			,IsCurrent
			,MatchingID
		) 
		select 
			@NewStageTypeID
			,null
			,@NewIterationNumber
			,@CurDate
			,null
			,1
			,@MatchingID
		;

	commit tran ; 

	return;

END
");
        }
    }
}
