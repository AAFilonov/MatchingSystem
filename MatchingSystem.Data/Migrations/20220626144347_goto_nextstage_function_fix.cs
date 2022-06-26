using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingSystem.Data.Migrations
{
    public partial class goto_nextstage_function_fix : Migration
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
Раскрыть
2closeAndNew.sql
6 кб
﻿
USE [DiplomaMatching]
GO
/****** Object:  StoredProcedure [napp].[goto_NextStage]    Script Date: 25.06.2022 22:42:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 15.05.2020
-- Description:	
-- =============================================
ALTER   PROCEDURE [napp].[goto_NextStage]
	@MatchingID int 
AS
BEGIN
	
	declare @ErrorMessage nvarchar (max); 

	if (@MatchingID is null) 
		throw 50001, 'Недопустимый NULL параметр: Параметр [MatchingID] не поддерживает значение NULL', 1; 

	declare 
		@CurStageID int
		,@CurIterationNumber int 
		,@CurStageTypeCode int
		,@CurDate datetime; 

	select 
		@CurStageID  = CurStage.StageID
		,@CurIterationNumber  = CurStage.IterationNumber
		,@CurStageTypeCode = CurStage.StageTypeCode
	from 
		napp.get_CurrentStage_ByMatching(@MatchingID) CurStage
		

	if (@CurStageID is null) 
	begin 
		set @ErrorMessage = 'Внутренняя ошибка бизнес-логики: у распределение с [MatchingID] = ' 
							+ cast (@MatchingID as nvarchar(20))
							+ ' не задано текущего этапа (stage)'; 
		throw 50004, @ErrorMessage, 1;	
	end; 

	if (@CurStageTypeCode is null) 
	begin 
		set @ErrorMessage = 'Внутренняя ошибка бизнес-логики: у этапа с [StageID] = ' 
							+ cast (@CurStageID as nvarchar(20))
							+ ' не задан тип'; 
		throw 50004, @ErrorMessage, 1;	
	end; 

	set @CurDate = getdate(); 


		if (@CurStageTypeCode = 1 or @CurStageTypeCode = 2) -- инициализация распределения или подготовка к распределнию 
		begin
			begin tran; 

			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = null
														,@MatchingID = @MatchingID 
			;

			commit tran; 
			return; 
		end;  

		if (@CurStageTypeCode = 3) -- сбор предпочтений студентов
		begin 
		
			begin tran;

			--exec [napp_in].[create_StudentsPreferences_Auto] @MatchingID = @MatchingID;
		
			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = 1
														,@MatchingID = @MatchingID
			;

			exec [napp_in].[create_TutorsChoice_Auto] @MatchingID = @MatchingID;

			commit tran; 

			return; 	
		end;

		if (@CurStageTypeCode = 4) -- итерации
		begin 
		
			begin tran;

			begin
				exec [napp_in].[upd_StudentsPreference_IsUsed] @MatchingID = @MatchingID;

				declare @NewIterationNumber smallint = @CurIterationNumber + 1; 

				exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
															,@CurStageID = @CurStageID
															,@CurStageTypeCode = @CurStageTypeCode
															,@NewIterationNumber = @NewIterationNumber
															,@MatchingID = @MatchingID
				;

				exec [napp_in].[create_TutorsChoice_Auto] @MatchingID = @MatchingID;

			end; 

			-- проверить не завершилось ли распределение
			if not exists (		select  -- больше нет студентов не в квоте в списках преподавателей, т.е. преподавателям больше нечего выбирать
									1
								from 
									dbo.TutorsChoice Choice with (nolock)
								where 
									Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID)
									and 
									Choice.IsInQuota = 0
							)
			begin -- если завершилось сразу переходим на следующий этап

				set @CurStageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID);  	

				exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
															,@CurStageID = @CurStageID
															,@CurStageTypeCode = @CurStageTypeCode
															,@NewIterationNumber = null
															,@MatchingID = @MatchingID
				;

				-- копируем выбор с последней итерации на 5-ый этап
				exec [napp_in].[create_TutorsChoice_Copy]	@MatchingID = @MatchingID
															,@PreviousStageID = @CurStageID
				;
			end;  		
			commit tran; 
			return; 	
		end; 
		if (@CurStageTypeCode = 5) -- ручная корректировка
		begin 
		
			begin tran;

			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = null
														,@MatchingID = @MatchingID 
			;

			-- копируем выбор с 5-го этапа на 6-ой
			exec [napp_in].[create_TutorsChoice_Copy]	@MatchingID = @MatchingID
														,@PreviousStageID = @CurStageID
			;

			commit tran;  
		end;   
	return; 
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
USE [DiplomaMatching]
GO
/****** Object:  StoredProcedure [napp].[goto_NextStage]    Script Date: 26.06.2022 17:48:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 15.05.2020
-- Description:	
-- =============================================
ALTER PROCEDURE [napp].[goto_NextStage]
	@MatchingID int 
AS
BEGIN
	
	declare @ErrorMessage nvarchar (max); 

	if (@MatchingID is null) 
		throw 50001, 'Недопустимый NULL параметр: Параметр [MatchingID] не поддерживает значение NULL', 1; 

	declare 
		@CurStageID int
		,@CurIterationNumber int 
		,@CurStageTypeCode int
		,@CurDate datetime; 

	select 
		@CurStageID  = CurStage.StageID
		,@CurIterationNumber  = CurStage.IterationNumber
		,@CurStageTypeCode = CurStage.StageTypeCode
	from 
		napp.get_CurrentStage_ByMatching(@MatchingID) CurStage
		

	if (@CurStageID is null) 
	begin 
		set @ErrorMessage = 'Внутренняя ошибка бизнес-логики: у распределение с [MatchingID] = ' 
							+ cast (@MatchingID as nvarchar(20))
							+ ' не задано текущего этапа (stage)'; 
		throw 50004, @ErrorMessage, 1;	
	end; 

	if (@CurStageTypeCode is null) 
	begin 
		set @ErrorMessage = 'Внутренняя ошибка бизнес-логики: у этапа с [StageID] = ' 
							+ cast (@CurStageID as nvarchar(20))
							+ ' не задан тип'; 
		throw 50004, @ErrorMessage, 1;	
	end; 

	set @CurDate = getdate(); 


		if (@CurStageTypeCode = 2) -- подготовка к распределнию 
		begin
			begin tran; 

			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = null
														,@MatchingID = @MatchingID 
			;

			commit tran; 
			return; 
		end;  

		if (@CurStageTypeCode = 3) -- сбор предпочтений студентов
		begin 
		
			begin tran;

			--exec [napp_in].[create_StudentsPreferences_Auto] @MatchingID = @MatchingID;
		
			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = 1
														,@MatchingID = @MatchingID
			;

			exec [napp_in].[create_TutorsChoice_Auto] @MatchingID = @MatchingID;

			commit tran; 

			return; 	
		end;

		if (@CurStageTypeCode = 4) -- итерации
		begin 
		
			begin tran;

			begin
				exec [napp_in].[upd_StudentsPreference_IsUsed] @MatchingID = @MatchingID;

				declare @NewIterationNumber smallint = @CurIterationNumber + 1; 

				exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
															,@CurStageID = @CurStageID
															,@CurStageTypeCode = @CurStageTypeCode
															,@NewIterationNumber = @NewIterationNumber
															,@MatchingID = @MatchingID
				;

				exec [napp_in].[create_TutorsChoice_Auto] @MatchingID = @MatchingID;

			end; 

			-- проверить не завершилось ли распределение
			if not exists (		select  -- больше нет студентов не в квоте в списках преподавателей, т.е. преподавателям больше нечего выбирать
									1
								from 
									dbo.TutorsChoice Choice with (nolock)
								where 
									Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID)
									and 
									Choice.IsInQuota = 0
							)
			begin -- если завершилось сразу переходим на следующий этап

				set @CurStageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID);  	

				exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
															,@CurStageID = @CurStageID
															,@CurStageTypeCode = @CurStageTypeCode
															,@NewIterationNumber = null
															,@MatchingID = @MatchingID
				;

				-- копируем выбор с последней итерации на 5-ый этап
				exec [napp_in].[create_TutorsChoice_Copy]	@MatchingID = @MatchingID
															,@PreviousStageID = @CurStageID
				;



			end;
		
			commit tran; 

			return; 	
		end; 
		if (@CurStageTypeCode = 5) -- ручная корректировка
		begin 
		
			begin tran;

			exec [napp_in].[upd_Stage_CloseAndSetNew]	@CurDate = @CurDate
														,@CurStageID = @CurStageID
														,@CurStageTypeCode = @CurStageTypeCode
														,@NewIterationNumber = null
														,@MatchingID = @MatchingID 
			;

			-- копируем выбор с 5-го этапа на 6-ой
			exec [napp_in].[create_TutorsChoice_Copy]	@MatchingID = @MatchingID
														,@PreviousStageID = @CurStageID
			;

			commit tran; 

		end; 


	return;

END




");
        }
    }
}
