IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'napp_in')
    BEGIN
        EXEC('CREATE SCHEMA [napp_in]');
    END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'napp')
    BEGIN
        EXEC('CREATE SCHEMA [napp]');
    END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'dbo')
    BEGIN
        EXEC('CREATE SCHEMA [dbo]');
    END
GO
GO




/****** Object:  StoredProcedure [napp_in].[upd_StudentsPreference_IsUsed]    Script Date: 17.03.2024 12:29:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 22.03.2020
-- Update date: 04.04.2020
-- Description:	
-- =============================================
CREATE PROCEDURE [napp_in].[upd_StudentsPreference_IsUsed]
@MatchingID int
AS
BEGIN


    update
        dbo.StudentsPreferences
    set
        IsInUse = 0
      ,IsUsed = 1

    from
        dbo_v.AvailableStudentsPreferences  Preference with (nolock)

            join dbo.TutorsChoice Choice with (nolock) on
            Choice.PreferenceID = Preference.PreferenceID
                and
            Choice.StageID = (select napp_in.get_CurrentStageID_ByMatching (@MatchingID) )

    where
        Preference.MatchingID = @MatchingID
      and
        Choice.IsInQuota = 0
      and
        Preference.PreferenceID = dbo.StudentsPreferences.PreferenceID;


    return;

END

GO

/****** Object:  StoredProcedure [napp_in].[create_TutorsChoice_Copy]    Script Date: 17.03.2024 12:28:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 22.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [napp_in].[create_TutorsChoice_Copy]
@MatchingID int
,@PreviousStageID int

AS
BEGIN

    declare
        @CurStageID int =(select napp_in.get_CurrentStageID_ByMatching (@MatchingID))
    ;


    begin tran;
    insert into dbo.TutorsChoice
    (
      StudentID
    ,ProjectID
    ,SortOrderNumber
    ,IsInQuota
    ,IsChangeble
    ,TypeID
    ,PreferenceID
    ,IterationNumber
    ,StageID
    ,CreateDate
    ,UpdateDate
    ,IsFromPreviousIteration
    )
    select
        Choice.StudentID
         ,Choice.ProjectID
         ,Choice.SortOrderNumber
         ,Choice.IsInQuota
         ,Choice.IsChangeble
         ,Choice.TypeID
         ,Choice.PreferenceID
         ,null
         ,@CurStageID
         ,getdate()
         ,null
         ,null

    from
        dbo.TutorsChoice Choice with (nolock)
    where
        Choice.StageID = @PreviousStageID
    ;

    commit tran;

    return;

END
GO

GO

/****** Object:  StoredProcedure [napp_in].[upd_Stage_CloseAndSetNew]    Script Date: 17.03.2024 12:28:12 ******/
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
CREATE PROCEDURE [napp_in].[upd_Stage_CloseAndSetNew]
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

GO



GO

/****** Object:  StoredProcedure [napp_in].[create_TutorsChoice_Auto]    Script Date: 17.03.2024 12:12:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.03.2020
-- Update date: 11.05.2020
-- Description:	Выполняет вставку автоматического выбора преподавателей. Используется в начале итерации 
-- =============================================
CREATE OR ALTER PROCEDURE [napp_in].[create_TutorsChoice_Auto]
@MatchingID int
AS
BEGIN
    ---------------------------------------------------------------------------
    --           Автоматическое раскидывание студентов по квоте				 --
    ---------------------------------------------------------------------------

    -- Таблица Preferences, которы доступны преподавателям на текущей итерации 
    create table #CurIteration_StudentsPreferences
    (
        PreferenceID int
        ,StudentID int
        ,ProjectID int
        ,TutorID int
        ,IsInUse bit
        ,ProjectQuota_Qty smallint
        ,TutorQuota_Qty smallint
        ,IsInQuota bit
    );


    --begin tran ;

    with AvailableStudentsPreferences
             as
             (
                 select
                     Preference.PreferenceID
                      ,Preference.StudentID
                      ,Preference.ProjectID
                      ,Preference.OrderNumber
                      ,Preference.IsInUse
                      ,min(Preference.OrderNumber) over (partition by Preference.StudentID) as MinOrderNumber
                 from
                     dbo.StudentsPreferences Preference with (nolock)

                         join dbo.Students Student with(nolock) on
                         Student.StudentID = Preference.StudentID
                 where
                     Preference.IsAvailable = 1
                   and
                     Preference.IsUsed = 0
                   and
                     Student.MatchingID = @MatchingID
             )
    -- Сразу проставляем В Квоте для тех предпочтений, что приехали из прошлой итерации
    insert into #CurIteration_StudentsPreferences
    (
      PreferenceID
    ,StudentID
    ,ProjectID
    ,TutorID
    ,IsInUse
    ,ProjectQuota_Qty
    ,TutorQuota_Qty
    ,IsInQuota

    )

    select
        Preference.PreferenceID
         ,Preference.StudentID
         ,Preference.ProjectID
         ,Tutor.TutorID
         ,Preference.IsInUse
         ,Project.ProjectQuotaQty
         ,TutorQuota.Qty as TutorQuota_Qty
         ,iif(Preference.IsInUse = 1, 1, null)  --Если Preference была в IsInUse, значит он приехала из прошлой итерации (иначе - это новый студик в списке у препода) 

    from
        AvailableStudentsPreferences Preference

            join dbo.Projects Project with (nolock) on
            Project.ProjectID = Preference.ProjectID

            join dbo.Tutors Tutor with(nolock) on
            Tutor.TutorID = Project.TutorID

            join dbo_v.ActiveCommonQuotas TutorQuota with(nolock) on
            TutorQuota.TutorID = Tutor.TutorID

    where
        Preference.OrderNumber = Preference.MinOrderNumber
    ;


    ----Проставляем В квоте
    with AvailableStudentsPreferences
        as
        (
            select
                *
                 ,sum(iif(Preference.IsInQuota = 1, 1, 0)) over (partition by Preference.ProjectID) as ProjectQuota_FillQty
                 ,sum(iif(Preference.IsInQuota = 1, 1, 0)) over (partition by Preference.TutorID) as TutorQuota_FillQty
            from
                #CurIteration_StudentsPreferences Preference
        )
       ,AvailableStudentsPreferences_ProjectQuotaConstraint -- определяем тех, ктоне пролез по квоте проекта
        as
        (
            select
                Preference.*
                 ,ProjectQuota_FreeQty
                 ,TutorQuota_FreeQty

                 , iif (
                    row_number() over  (partition by Preference.ProjectID order by Preference.PreferenceID) <= ProjectQuota_FreeQty
                , 1
                , 0
                   ) as IsAvailable_ByProjectQuota

            from
                AvailableStudentsPreferences Preference

                    cross apply
                (
                    select
                        coalesce(Preference.ProjectQuota_Qty, Preference.TutorQuota_Qty) - Preference.ProjectQuota_FillQty as ProjectQuota_FreeQty
                         ,Preference.TutorQuota_Qty - Preference.TutorQuota_FillQty as TutorQuota_FreeQty
                ) as calc01

            where
                Preference.IsInQuota is null
        )
       ,AvailableStudentsPreferences_Quota -- проставляем в квоте или нет на основе квоты преподавателя. 
        as
        (
            select
                Preference.PreferenceID
                 --,row_number() over  (partition by Preference.TutorID order by Preference.IsAvailable_ByProjectQuota desc) as StudernOrder_Tutor

                 , iif (
                    Preference.IsAvailable_ByProjectQuota = 1 --Тех кто отмечен невлазящим в квоту проекта, помечаем как не в квоте без проверки квоты препода
                        and
                    row_number() over  (partition by Preference.TutorID order by Preference.IsAvailable_ByProjectQuota desc) <= TutorQuota_FreeQty
                ,1
                ,0
                   ) as IsInQuota_Calc

            from
                AvailableStudentsPreferences_ProjectQuotaConstraint Preference
        )
    update
        #CurIteration_StudentsPreferences
    set
        IsInQuota = IsInQuota_Calc
    from
        AvailableStudentsPreferences_Quota p
    where
        #CurIteration_StudentsPreferences.PreferenceID = p.PreferenceID
    ;


    --------------------------------------------------------------------------
    ---- Складывание результата в таблицу Choise и обновление Preferences

    declare
        @CurIterationNumber int
        ,@CurStageID int
        ,@AutoChoosingTypeID int = (select TypeID from dbo.ChoosingTypes where TypeName = 'Auto');

    select
        @CurIterationNumber = IterationNumber
         ,@CurStageID = StageID
    from
        [napp].[get_CurrentStage_ByMatching] (@MatchingID);

    insert into dbo.TutorsChoice
    (
      StudentID
    ,ProjectID
    ,SortOrderNumber
    ,IsInQuota
    ,IsChangeble
    ,TypeID
    ,PreferenceID
    ,IterationNumber
    ,StageID
    ,CreateDate
    ,UpdateDate
    ,IsFromPreviousIteration
    )
    select
        Preference.StudentID
         ,Preference.ProjectID
         ,coalesce(LastChoise.SortOrderNumber, 32767)
         ,Preference.IsInQuota
         ,coalesce(LastChoise.IsChangeble, 1)
         ,coalesce(LastChoise.TypeID, @AutoChoosingTypeID)
         ,Preference.PreferenceID
         ,@CurIterationNumber
         ,@CurStageID
         ,getdate()
         ,null
         ,iif(LastChoise.ChoiceID is not null, 1, 0)

    from
        #CurIteration_StudentsPreferences Preference

            left join dbo.TutorsChoice LastChoise with (nolock) on
            LastChoise.PreferenceID = Preference.PreferenceID
                and
            LastChoise.IterationNumber = @CurIterationNumber - 1
                and
            LastChoise.IsInQuota = 1
    ;

    update
        dbo.StudentsPreferences
    set
        IsInUse = 1
    from
        #CurIteration_StudentsPreferences
    where
        dbo.StudentsPreferences.PreferenceID = #CurIteration_StudentsPreferences.PreferenceID
    ;

    --commit tran;

    drop table #CurIteration_StudentsPreferences;


    return;

END




GO

/****** Object:  StoredProcedure [napp].[goto_NextStage]    Script Date: 17.03.2024 12:25:53 ******/
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
CREATE   PROCEDURE [napp].[goto_NextStage]
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

GO

