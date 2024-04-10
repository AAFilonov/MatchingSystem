GO
/****** Object:  User [diplomamatchinguser]    Script Date: 17.03.2024 12:16:12 ******/
CREATE USER [diplomamatchinguser] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [WebAppDiplomaMatching]    Script Date: 17.03.2024 12:16:12 ******/
CREATE USER [WebAppDiplomaMatching] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [diplomamatchinguser]
GO
ALTER ROLE [db_owner] ADD MEMBER [WebAppDiplomaMatching]
GO
ALTER ROLE [db_datareader] ADD MEMBER [WebAppDiplomaMatching]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [WebAppDiplomaMatching]
GO
/****** Object:  Schema [dbo_v]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [dbo_v]
    GO
/****** Object:  Schema [glob]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [glob]
    GO
/****** Object:  Schema [napp]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [napp]
    GO
/****** Object:  Schema [napp_in]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [napp_in]
    GO
/****** Object:  Schema [stg]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [stg]
    GO
/****** Object:  Schema [tmp]    Script Date: 17.03.2024 12:16:12 ******/
CREATE SCHEMA [tmp]
    GO
/****** Object:  UserDefinedTableType [dbo].[ProjectQuota]    Script Date: 17.03.2024 12:16:12 ******/
CREATE TYPE [dbo].[ProjectQuota] AS TABLE(
    [ProjectID] [int] NOT NULL,
    [Quota] [smallint] NULL
    )
    GO
/****** Object:  UserDefinedTableType [dbo].[StrList]    Script Date: 17.03.2024 12:16:12 ******/
CREATE TYPE [dbo].[StrList] AS TABLE(
    [value] [nvarchar](max) NULL,
    [id] [int] IDENTITY(1,1) NOT NULL
    )
    GO
/****** Object:  UserDefinedTableType [dbo].[TutorsChoice_1]    Script Date: 17.03.2024 12:16:12 ******/
CREATE TYPE [dbo].[TutorsChoice_1] AS TABLE(
    [ChoiseID] [int] NULL,
    [SortOrderNumber] [smallint] NULL,
    [IsInQuota] [bit] NULL
    )
    GO
/****** Object:  UserDefinedFunction [dbo].[agg_StrList]    Script Date: 17.03.2024 12:16:12 ******/
    


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 19.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[agg_StrList]
(
	@StrList dbo.StrList readonly
	,@Separator nvarchar(100) 
)
RETURNS nvarchar(max)
AS
BEGIN
	
	declare
@MaxID int = (select max(id) from @StrList)
		,@CurID int = 1
		,@AggStr nvarchar(max) = '';

	while (@CurID < @MaxID)
begin 
		set @AggStr = @AggStr 
						+ (select value from @StrList where id = @CurID) 
						+ @Separator
		;
		set @CurID = @CurID + 1 ;

end

	set @AggStr = @AggStr 
					+ (select value from @StrList where id = @MaxID) 
	
	return @AggStr;

END



GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 19.02.2020
-- Update date: 13.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_ByTutor]
(
	@TutorID int
)
RETURNS int
AS
BEGIN
	declare
@CommonQuotaQty int
	; 

	set @CommonQuotaQty = 
		(
			select top 1 
				q.Qty
			from 
				dbo_v.ActiveCommonQuotas q with (nolock) 
			where 
				q.TutorID = @TutorID 
		)
	;


return coalesce( @CommonQuotaQty, -1);


END




GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_Requests_NotificationCount_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 24.02.2020
-- Update date: 10.04.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_Requests_NotificationCount_ByExecutive]
(
	@UserID int
	,@MatchingID int
)
RETURNS int
AS
BEGIN
	declare @NotificationCount int; 

	if not exists (	select 1 
					from 
						dbo.Users_Roles ExecutiveUser with (nolock) 
						
						join  dbo.Roles ExecutiveRole with (nolock) on 
							ExecutiveRole.RoleCode = 3 --executive
							and 
							ExecutiveRole.RoleID = ExecutiveUser.RoleID
					where 
						ExecutiveUser.UserID = @UserID
						and 
						ExecutiveUser.MatchingID = @MatchingID
						)
		return -1; 


	set @NotificationCount = (	select count(*) 
								from 
									dbo.Tutors t with(nolock)  

									join dbo.CommonQuotas q with(nolock)  on 
										q.TutorID = t.TutorID 

									join dbo.QuotasStates qs with (nolock) on 
										qs.QuotaStateID = q.QuotaStateID
								where 
									t.MatchingID = @MatchingID
									and 
									qs.QuotaStateName = 'requested'		
									-------------------------------
									and 
									q.IsNotification = 1		
									------------------------------				
								)
	
	return coalesce(@NotificationCount, 0);


END



GO
/****** Object:  UserDefinedFunction [napp].[get_IsCommonQuotaNotification_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 12.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_IsCommonQuotaNotification_ByExecutive]
(
	@UserID int
	,@MatchingID int
)
RETURNS bit
AS
BEGIN

	if not exists (	select 1 
					from 
						dbo.Users_Roles ExecutiveUser with (nolock) 
						
						join  dbo.Roles ExecutiveRole with (nolock) on 
							ExecutiveRole.RoleCode = 3 --executive
							and 
							ExecutiveRole.RoleID = ExecutiveUser.RoleID
					where 
						ExecutiveUser.UserID = @UserID
						and 
						ExecutiveUser.MatchingID = @MatchingID
					)
		return 0; 

	if exists (		select 
						1
					from 
						dbo.Tutors t with(nolock)  

						join dbo.CommonQuotas q with(nolock)  on 
							q.TutorID = t.TutorID 

						join dbo.QuotasStates qs with (nolock) on 
							qs.QuotaStateID = q.QuotaStateID
					where 
						t.MatchingID = @MatchingID
						and 
						qs.QuotaStateName = 'requested'		
						and 
						q.IsNotification = 1 	
				)
		return 1;

return 0;




END




GO
/****** Object:  UserDefinedFunction [napp].[get_IsCommonQuotaNotification_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 12.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_IsCommonQuotaNotification_ByTutor]
(
	@TutorID int
)
RETURNS bit
AS
BEGIN

	if exists (		select 
						1
					from dbo.CommonQuotas q with(nolock)  

						join dbo.QuotasStates qs with (nolock) on 
							qs.QuotaStateID = q.QuotaStateID
					where 
						q.TutorID = @TutorID
						and 
						qs.QuotaStateName != 'requested'		
						and 
						q.IsNotification = 1 	
				)
		return 1;

return 0;




END




GO
/****** Object:  UserDefinedFunction [napp].[get_IsReadyToStart_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 24.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_IsReadyToStart_ByTutor]
(
	@TutorID int
)
RETURNS int
AS
BEGIN
	declare
@IsReady bit
	; 

	set @IsReady = 
		(
			select top 1 
				Tutor.IsReadyToStart
			from 
				dbo.Tutors Tutor with (nolock) 
			where 
				Tutor.TutorID = @TutorID 
		)
	;

return @IsReady;


END



GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage2_Main]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 29.02.2020
-- Update date: 06.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StatisticStage2_Main]
(	
	@MatchingID int
)

RETURNS @Statistic TABLE 
(
	StatName			nvarchar(300) 
	,StatValue			int 
	,StatValueFrom		int
	,StatValue_Str		nvarchar(20) 
	,StatPercentage		numeric(6, 2)

)
AS
begin 
	if (@MatchingID is null)
		return; 

	declare
@TutorsAll int 
		,@TutorsVisitedOnCurStage int 
		,@TutorsReadyToStart int
		,@CommonQuotaSum int
		,@CommonQuotaAvg int
		,@StudentsAll int
		,@StudentsVisitedOnCurStage int
		,@ProjectsAll int
		,@ProjectsAvg int
	;

select
    @TutorsAll = count (*)
     ,@TutorsVisitedOnCurStage = count(	case
                                               when Tutor.LastVisitDate >= CurrentStage.StartDate  then
                                                   1
                                               else
                                                   null
    end)
     ,@TutorsReadyToStart = count(	case
                                          when Tutor.IsReadyToStart = 1 then
                                              1
                                          else
                                              null
    end)
     ,@CommonQuotaSum = sum(Quota.Qty)
     ,@CommonQuotaAvg = avg(Quota.Qty)
from dbo_v.Tutors Tutor

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage

		--join dbo.Projects DefaultProject with (nolock) on 
		--	DefaultProject.TutorID = Tutor.TutorID
		--	and 
		--	DefaultProject.IsDefault = 1

		join dbo_v.ActiveCommonQuotas Quota on
    Quota.TutorID = Tutor.TutorID
--and 
--Quota.IsCommon = 1
where
    Tutor.MatchingID = @MatchingID
;


select
    @StudentsAll = count(*)
     ,@StudentsVisitedOnCurStage = count(case
                                             when Student.LastVisitDate >= CurrentStage.StartDate  then
                                                 1
                                             else
                                                 null
    end)
from
    dbo_v.Students Student

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage
where
    Student.MatchingID = @MatchingID
;

select
    @ProjectsAll = count (ProjectID)
     ,@ProjectsAvg = count (ProjectID) / count (distinct TutorID)
from
    dbo.Projects with (nolock)
where
    MatchingID = @MatchingID
;


insert into @Statistic
(
  StatName
,StatValue
,StatValueFrom
,StatValue_Str
,StatPercentage
)
select
    'Участвуют в распределении: преподавателей' as StatName
     ,@TutorsAll	as StatValue
     ,null		as StatValueFrom
     ,cast (@TutorsAll as nvarchar (20) ) 	 as StatValue_Str
     ,null		as StatPercentage

union all

select
    'Участвуют в распределении: студентов'
     ,@StudentsAll
     ,null
     ,cast (@StudentsAll as nvarchar (20) )
     ,null

union all

select
    'Посетили систему на текущем этапе: Преподавателей'
     ,@TutorsVisitedOnCurStage
     ,@TutorsAll
     ,cast (@TutorsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@TutorsAll as nvarchar (10) )

     ,round (cast(@TutorsVisitedOnCurStage as numeric(5, 1))
                 /
             cast(@TutorsAll as numeric(5, 1))
    ,2)

union all

select
    'Посетили систему на текущем этапе: Студентов'
     ,@StudentsVisitedOnCurStage
     ,@StudentsAll
     ,cast (@StudentsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsVisitedOnCurStage as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Готово к распределению: Преподавателей'
     ,@TutorsReadyToStart
     ,@TutorsAll
     ,cast (@TutorsReadyToStart  as nvarchar (10) )
    + ' / '
    + cast (@TutorsAll as nvarchar (10) )
     ,round (
        cast(@TutorsReadyToStart  as numeric(5, 1))
            /
        cast(@TutorsAll as numeric(5, 1))
    ,2)

union all

select
    'Квота: сумма'
     ,@CommonQuotaSum
     ,null
     ,cast (@CommonQuotaSum  as nvarchar (20))
     ,null

union all

select
    'Квота: средняя'
     ,@CommonQuotaAvg
     ,null
     ,cast (@CommonQuotaAvg  as nvarchar (20))
     ,null

union all

select
    'Проектов: всего'
     ,@ProjectsAll
     ,null
     ,cast (@ProjectsAll as nvarchar (20) )
     ,null

union all

select
    'Проектов: в среднем на преподавателя'
     ,@ProjectsAvg
     ,null
     ,cast (@ProjectsAvg as nvarchar (20) )
     ,null

;

return;

end

GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Main]    Script Date: 17.03.2024 12:16:12 ******/




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 07.03.2020
-- Update date: 11.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StatisticStage3_Main]
(	
	@MatchingID int
)

RETURNS @Statistic TABLE 
(
	StatName			nvarchar(300) 
	,StatValue			int 
	,StatValueFrom		int
	,StatValue_Str		nvarchar(max) 
	,StatPercentage		numeric(6, 2)

)
AS
begin 
	if (@MatchingID is null)
		return; 

	declare
@TutorsAll int 
		,@TutorsVisitedOnCurStage int 
		,@StudentsAll int
		,@StudentsVisitedOnCurStage int
		,@StudentsWithPreferences int
		,@ProjectsPreferencesAvg int
		

	;

	--=========== Tutors ===========-
select
    @TutorsAll = count (*)
     ,@TutorsVisitedOnCurStage = count(	case
                                               when Tutor.LastVisitDate >= CurrentStage.StartDate  then
                                                   1
                                               else
                                                   null
    end)
from dbo_v.Tutors Tutor

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage

where
    Tutor.MatchingID = @MatchingID
;

--=========== Students ===========-
select
    @StudentsAll = count(Student.StudentID)
     ,@StudentsVisitedOnCurStage = count(case
                                             when Student.LastVisitDate >= CurrentStage.StartDate  then
                                                 1
                                             else
                                                 null
    end)
     ,@StudentsWithPreferences = count (Preference.StudentID)
     ,@ProjectsPreferencesAvg = avg (Preference.CountProjects)


from
    dbo_v.Students Student

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage

		left join
				(
					select  
						StudentID 
						, count(PreferenceID) as CountProjects
					from 
						dbo.StudentsPreferences  with (nolock)
					group by 
						StudentID
				) as Preference
on Preference.StudentID = Student.StudentID

where
    Student.MatchingID = @MatchingID
;

--=========== Top 3 ===========-
declare
@TopProjects dbo.StrList
		,@TopTutors  dbo.StrList;

insert into @TopProjects (value)
select top 3
    coalesce (Project.ProjectName, 'Записаться к преподавателю') 
		+ ' : ' + Tutor.NameAbbreviation + ' (' 
		+ cast (count(Preference.PreferenceID) as nvarchar (10)) + ')'

from
    dbo.Projects Project with (nolock)

    join dbo_v.Tutors Tutor with (nolock) on
    Tutor.TutorID = Project.TutorID

    join dbo.StudentsPreferences Preference with (nolock) on
    Preference.ProjectID = Project.ProjectID
where
    Project.MatchingID = @MatchingID
group by
    Project.ProjectName
        ,Tutor.NameAbbreviation

order by
    count(Preference.PreferenceID) desc;

insert into @TopTutors (value)
select top 3 
		Tutor.NameAbbreviation
		+ ' (' + cast (count(Preference.PreferenceID) as nvarchar (10)) + ')'

from  dbo_v.Tutors Tutor with (nolock)

		join dbo.Projects Project with (nolock) on
    Project.TutorID = Tutor.TutorID

    join dbo.StudentsPreferences Preference with (nolock) on
    Preference.ProjectID = Project.ProjectID


where
    Tutor.MatchingID = @MatchingID
group by
    Tutor.NameAbbreviation

order by
    count(Preference.PreferenceID) desc
;


--========== Заполнение статистики ==========--
insert into @Statistic
(
  StatName
,StatValue
,StatValueFrom
,StatValue_Str
,StatPercentage
)
select
    'Участвуют в распределении: преподавателей' as StatName
     ,@TutorsAll	as StatValue
     ,null		as StatValueFrom
     ,cast (@TutorsAll as nvarchar (20) ) 	 as StatValue_Str
     ,null		as StatPercentage

union all

select
    'Участвуют в распределении: студентов'
     ,@StudentsAll
     ,null
     ,cast (@StudentsAll as nvarchar (20) )
     ,null

union all

select
    'Посетили систему на текущем этапе: Преподавателей'
     ,@TutorsVisitedOnCurStage
     ,@TutorsAll
     ,cast (@TutorsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@TutorsAll as nvarchar (10) )

     ,round (cast(@TutorsVisitedOnCurStage as numeric(5, 1))
                 /
             cast(@TutorsAll as numeric(5, 1))
    ,2)

union all

select
    'Посетили систему на текущем этапе: Студентов'
     ,@StudentsVisitedOnCurStage
     ,@StudentsAll
     ,cast (@StudentsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsVisitedOnCurStage as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Сделали выбор: Студентов'
     ,@StudentsWithPreferences
     ,@StudentsAll
     ,cast (@StudentsWithPreferences  as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsWithPreferences  as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Среднее количество выбранных проектов'
     ,@ProjectsPreferencesAvg
     ,null
     ,cast (@ProjectsPreferencesAvg  as nvarchar (20))
     ,null

union all

select
    'Топ 3: Проекты'
     ,null
     ,null
     ,dbo.agg_StrList(@TopProjects, ', ')
     ,null

union all

select
    'Топ 3: Преподаватели'
     ,null
     ,null
     ,dbo.agg_StrList(@TopTutors, ', ')
     ,null
;

return;

end


GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Main_ProgressBars]    Script Date: 17.03.2024 12:16:12 ******/


CREATE FUNCTION [napp].[get_StatisticStage3_Main_ProgressBars]
(	
	@MatchingID int
)

RETURNS @Statistic TABLE 
(
	StatName			nvarchar(300) 
	,StatValue			int 
	,StatValueFrom		int
	,StatValue_Str		nvarchar(20) 
	,StatPercentage		numeric(6, 2)

)
AS
begin 
	if (@MatchingID is null)
		return;

with StatGroup
         as
         (
             select
    [Group].GroupID
        ,[Group].GroupName
        ,count(Student.StudentID) as StudentAll
        ,count(Preference.StudentID) as StudentWithPreferences
from
    dbo.Groups [Group] with (nolock)

    join dbo.Students Student with (nolock) on
    Student.GroupID = [Group].GroupID

    left join
    (
    select distinct
    StudentID
    from
    dbo.StudentsPreferences  with (nolock)
    ) as Preference
on Preference.StudentID = Student.StudentID


where
    [Group].MatchingID = @MatchingID
group by
    [Group].GroupID
        ,[Group].GroupName
    )

insert into @Statistic
(
  StatName
,StatValue
,StatValueFrom
,StatValue_Str
,StatPercentage
)

select
    GroupName
     ,StudentWithPreferences
     ,StudentAll
     ,cast (StudentWithPreferences as nvarchar (10) )
    + ' / '
    + cast (StudentAll as nvarchar (10) )

     ,round (cast(StudentWithPreferences as numeric(5, 1))
                 /
             cast(StudentAll as numeric(5, 1))
    ,2)
from StatGroup

         return;

end


GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage4_Main]    Script Date: 17.03.2024 12:16:12 ******/





-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.05.2020
-- Update date: 11.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StatisticStage4_Main]
(	
	@MatchingID int
)

RETURNS @Statistic TABLE 
(
	StatName			nvarchar(300) 
	,StatValue			int 
	,StatValueFrom		int
	,StatValue_Str		nvarchar(max) 
	,StatPercentage		numeric(6, 2)

)
AS
begin 
	if (@MatchingID is null)
		return; 

	declare
@TutorsAll int 
		,@TutorsVisitedOnCurStage int 
		,@TutorsWithAvailableChoice int 
		,@TutorsWithSelfChoice int

		,@StudentsAll int
		,@StudentsVisitedOnCurStage int
		,@StudentsAllocated int 
		,@StudentsCantAllocated int
		,@StudentsInQuota int

		,@ProjectsAll int
		,@ProjectClosed int 
		

	;

	--=========== Tutors ===========-
select
    @TutorsAll = count (*)
     ,@TutorsVisitedOnCurStage = count(	case
                                               when Tutor.LastVisitDate >= CurrentStage.StartDate  then
                                                   1
                                               else
                                                   null
    end)

     ,@TutorsWithAvailableChoice = count (case when (coalesce(Choice.NotInQuotaCount, 0)!=0) then 1 end)
     ,@TutorsWithSelfChoice = count (case when (coalesce(Choice.SelfChoiceCount, 0)!=0) then 1 end)

from dbo_v.Tutors Tutor

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage

		left join	(
						select 
							Project.TutorID as TutorID
							, count (case when TutorChoice.IsInQuota = 0 then 1 end) as NotInQuotaCount
							, count (case when CType.TypeName = 'Self' then 1 end) as SelfChoiceCount
						
						from 
							dbo.TutorsChoice TutorChoice with (nolock)
						
							join dbo.Projects Project with (nolock) on 
								Project.ProjectID = TutorChoice.ProjectID 

							join dbo.ChoosingTypes CType with (nolock) on 
								CType.TypeID = TutorChoice.TypeID

						where 
							Project.MatchingID = @MatchingID 
							and 
							TutorChoice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID) 
						group by 
							Project.TutorID
				) as Choice on
    Choice.TutorID = Tutor.TutorID

where
    Tutor.MatchingID = @MatchingID
;

--=========== Students ===========-
select
    @StudentsAll = count(Student.StudentID)
     ,@StudentsVisitedOnCurStage = count(case
                                             when Student.LastVisitDate >= CurrentStage.StartDate  then
                                                 1
                                             else
                                                 null
    end)
     ,@StudentsInQuota = count (case when TutorChoice.IsInQuota = 1 then TutorChoice.ChoiceID end)
     ,@StudentsAllocated = count (AllocatedProject.ProjectID)
     ,@StudentsCantAllocated = count (case when TutorChoice.ChoiceID is null then 1 end)


from
    dbo_v.Students Student

    cross apply 
		(
			select * 
			from 
				napp.get_CurrentStage_ByMatching(@MatchingID) 
		) CurrentStage

		left join dbo.TutorsChoice TutorChoice with (nolock) on
    TutorChoice.StudentID = Student.StudentID
    and
    TutorChoice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID)
    --and 
    --TutorChoice.IsInQuota = 1

    left join dbo.Projects AllocatedProject with (nolock) on
    AllocatedProject.ProjectID = TutorChoice.ProjectID
    and
    AllocatedProject.IsClosed = 1
    and
    TutorChoice.IsInQuota = 1

where
    Student.MatchingID = @MatchingID
;

--=========== Проекты ===========-
select
    @ProjectsAll = count(*)
     ,@ProjectClosed = count( case when Project.IsClosed = 1 then 1 end)
from
    dbo.Projects Project with (nolock)
where
    Project.MatchingID = @MatchingID
;

--========== Заполнение статистики ==========--
insert into @Statistic
(
  StatName
,StatValue
,StatValueFrom
,StatValue_Str
,StatPercentage
)
select
    'Участвуют в распределении: преподавателей' as StatName
     ,@TutorsAll	as StatValue
     ,null		as StatValueFrom
     ,cast (@TutorsAll as nvarchar (20) ) 	 as StatValue_Str
     ,null		as StatPercentage

union all

select
    'Участвуют в распределении: студентов'
     ,@StudentsAll
     ,null
     ,cast (@StudentsAll as nvarchar (20) )
     ,null

union all

select
    'Посетили систему на текущем этапе: Преподавателей'
     ,@TutorsVisitedOnCurStage
     ,@TutorsAll
     ,cast (@TutorsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@TutorsAll as nvarchar (10) )

     ,round (cast(@TutorsVisitedOnCurStage as numeric(5, 1))
                 /
             cast(@TutorsAll as numeric(5, 1))
    ,2)

union all

select
    'Посетили систему на текущем этапе: Студентов'
     ,@StudentsVisitedOnCurStage
     ,@StudentsAll
     ,cast (@StudentsVisitedOnCurStage as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsVisitedOnCurStage as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Студентов распределено'
     ,@StudentsAllocated
     ,@StudentsAll
     ,cast (@StudentsAllocated  as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsAllocated  as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Студентов потенциально распределено'
     ,@StudentsInQuota  - @StudentsAllocated
     ,@StudentsAll
     ,cast (@StudentsInQuota - @StudentsAllocated  as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsInQuota - @StudentsAllocated  as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Студентов не может быть распределено'
     ,@StudentsCantAllocated
     ,@StudentsAll
     ,cast (@StudentsCantAllocated  as nvarchar (10) )
    + ' / '
    + cast (@StudentsAll as nvarchar (10) )
     ,round (
        cast(@StudentsCantAllocated  as numeric(5, 1))
            /
        cast(@StudentsAll as numeric(5, 1))
    ,2)

union all

select
    'Проектов закрыто'
     ,@ProjectClosed
     ,@ProjectsAll
     ,cast (@ProjectClosed  as nvarchar (10) )
    + ' / '
    + cast (@ProjectsAll as nvarchar (10) )
     ,round (
        cast(@ProjectClosed  as numeric(5, 1))
            /
        cast(@ProjectsAll as numeric(5, 1))
    ,2)
;

return;

end




GO
/****** Object:  UserDefinedFunction [napp].[get_StudentID]    Script Date: 17.03.2024 12:16:12 ******/




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 04.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StudentID]
(
	@UserID int
	,@ManchingID int
)
RETURNS int
AS
BEGIN
	declare
@StudentID int
	; 

	set @StudentID = 
		(
			select top 1 UserRole.StudentID
				
			from 
				dbo.Users_Roles UserRole with (nolock) 

				join dbo.Roles [Role] with (nolock) on 
					[Role].RoleID = UserRole.RoleID

			where 
				UserRole.UserID = @UserID 
				and
				UserRole.MatchingID = @ManchingID 
				and
				[Role].RoleCode = 2 -- Student
				
		)
	;

return coalesce(@StudentID, -1);


END




GO
/****** Object:  UserDefinedFunction [napp].[get_TutorID]    Script Date: 17.03.2024 12:16:12 ******/




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 04.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_TutorID]
(
	@UserID int
	,@ManchingID int
)
RETURNS int
AS
BEGIN
	declare
@TutorID int
	; 

	set @TutorID = 
		(
			select top 1 UserRole.TutorID
				
			from 
				dbo.Users_Roles UserRole with (nolock) 

				join dbo.Roles [Role] with (nolock) on 
					[Role].RoleID = UserRole.RoleID

			where 
				UserRole.UserID = @UserID 
				and
				UserRole.MatchingID = @ManchingID 
				and
				[Role].RoleCode = 1 -- Tutor
				
		)
	;

return coalesce(@TutorID, -1);


END




GO
/****** Object:  UserDefinedFunction [napp].[get_User]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 31.01.2020
-- Update date: 11.02.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_User]
(	
	@UserID int = null
	,@Login nvarchar(50) = null 
)
RETURNS @User TABLE 
(
	UserID int 
	,[Login] nvarchar(50)
	,LastVisitDate datetime
	,Email nvarchar(50) 
	,[Name] nvarchar(100)
	,[Surname] nvarchar(100)
	,[Patronimic] nvarchar(100) 
	,NameAbbreviation nvarchar(106) 
)
AS
begin 
	if ((@UserID is null) and (@Login is null)) 
		return;

insert into @User
(
  UserID
,[Login]
,LastVisitDate
,Email
,[Name]
,[Surname]
,[Patronimic]
,NameAbbreviation
)
select top 1
    [User].UserID
		,[User].Login
		,[User].LastVisitDate
		,[User].Email
		,[User].Name
		,[User].Surname
		,[User].Patronimic
		,[User].Surname 
			+ ' ' + iif ([User].Name is not null, substring([User].Name,1, 1) , '' ) + '.'
			+ ' ' + iif ([User].Patronimic is not null, substring([User].Patronimic,1, 1) , '' ) + '.'
				as NameAbbreviation

from
    dbo.Users [User] with (nolock)

where
    [User].UserID = coalesce(@UserID, [User].UserID)
  and
    [User].[Login] = coalesce(@Login, [User].[Login])
;

return;

end
GO
/****** Object:  UserDefinedFunction [napp].[get_UserID]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 31.01.2020
-- Update date: 11.02.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserID]
(
	@Login nvarchar (50)
)
RETURNS int
AS
BEGIN
	declare
@UserID int
	; 

	set @UserID = 
		(
			select top 1 
				u.UserID
			from 
				dbo.Users u
			where 
				u.[Login] = @Login
		)
	; 

	if (@UserID is not null)
		return @UserID;

return -1;


END
GO
/****** Object:  UserDefinedFunction [napp].[get_UserMatchings]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 31.01.2020
-- Update date: 13.02.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserMatchings]
(	
	@UserID int = null
	,@Login nvarchar(50) = null 
)
RETURNS @UserMatchings TABLE 
(
	UserID int 
	,MatchingID int 
	,MatchingName nvarchar(100)
	,MatchingTypeCode int
	,MatchingTypeName nvarchar(50)
	,MatchingTypeName_ru nvarchar(50)


)
AS
begin 
	if ((@UserID is null) and (@Login is null)) 
		return;

insert into @UserMatchings
(
  UserID
,MatchingID
,MatchingName
,MatchingTypeCode
,MatchingTypeName
,MatchingTypeName_ru
)
select distinct
    [User].UserID

        ,Match.MatchingID
        ,Match.MatchingName

        ,MatchType.MatchingTypeCode
        ,MatchType.MatchingTypeName
        ,MatchType.MatchingTypeName_ru

from
    dbo.Users [User] with (nolock)

    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.UserID = [User].UserID

    join dbo.Matching Match with (nolock) on
    Match.MatchingID = UserRole.MatchingID

    join dbo.MatchingType MatchType with (nolock) on
    MatchType.MatchingTypeID = Match.MatchingTypeID
where
    [User].UserID = coalesce(@UserID, [User].UserID)
  and
    [User].[Login] = coalesce(@Login, [User].[Login])
;

return;

end
GO
/****** Object:  UserDefinedFunction [napp].[get_UserPasswordHash]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 12.02.2020
-- Update date: 01.03.2020
-- Description:	Возвращает хэш паролья по логину пользователя
-- =============================================
CREATE FUNCTION [napp].[get_UserPasswordHash]
(
	@Login nvarchar (50)
)
RETURNS nvarchar (max)
AS
BEGIN
	declare
@Hash nvarchar (max)
	; 

	set @Hash = 
		(
			select top 1 
				u.PasswordHash
			from 
				dbo.Users u
			where 
				u.[Login] = @Login
		)
	; 

	if (@Hash is not null)
		return @Hash;

return '-1';


END

GO
/****** Object:  UserDefinedFunction [napp].[get_UserRoles]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 31.01.2020
-- Update date: 11.02.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserRoles]
(	
	@UserID int = null
	,@Login nvarchar(50) = null 
)
RETURNS @UserRoles TABLE 
(
	UserID int 
	,RoleCode int
	,RoleName nvarchar(50)
	,RoleName_ru nvarchar(50)

)
AS
begin

	if ((@UserID is null) and (@Login is null)) 
		return;

insert into @UserRoles
(
  UserID
,RoleCode
,RoleName
,RoleName_ru
)
select distinct
    [User].UserID
        ,[Role].RoleCode
        ,[Role].RoleName
        ,[Role].RoleName_ru

from
    dbo.Users [User] with (nolock)

    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.UserID = [User].UserID

    join dbo.Roles [Role] with (nolock) on
    [Role].RoleID = UserRole.RoleID

where
    [User].UserID = coalesce(@UserID, [User].UserID)
  and
    [User].[Login] = coalesce(@Login, [User].[Login])
;

return;

end
GO
/****** Object:  UserDefinedFunction [napp].[get_UserRolesMatchings]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 31.01.2020
-- Update date: 04.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserRolesMatchings]
(	
	@UserID int = null
	,@Login nvarchar(50) = null 
)
RETURNS @UserRolesMatchings TABLE 
(
	UserID int 
	,RoleCode int
	,RoleName nvarchar(50)
	,RoleName_ru nvarchar(50)
	,MatchingID int
	,MatchingName nvarchar(100)
	,MatchingTypeCode int
	,MatchingTypeName nvarchar(50)
	,MatchingTypeName_ru nvarchar(50)
	,TutorID int 
	,StudentID int

)
AS
begin

	if ((@UserID is null) and (@Login is null)) 
		return;

insert into @UserRolesMatchings
(
  UserID
,RoleCode
,RoleName
,RoleName_ru
,MatchingID
,MatchingName
,MatchingTypeCode
,MatchingTypeName
,MatchingTypeName_ru
,TutorID
,StudentID
)
select
    [User].UserID

        ,[Role].RoleCode
        ,[Role].RoleName
        ,[Role].RoleName_ru

        ,Match.MatchingID
        ,Match.MatchingName

        ,MatchType.MatchingTypeCode
        ,MatchType.MatchingTypeName
        ,MatchType.MatchingTypeName_ru

        ,UserRole.TutorID
        ,UserRole.StudentID

from
    dbo.Users [User] with (nolock)
    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.UserID = [User].UserID
    join dbo.Roles [Role] with (nolock) on
    [Role].RoleID = UserRole.RoleID
    left join dbo.Matching Match with (nolock) on
    Match.MatchingID = UserRole.MatchingID
    left join dbo.MatchingType MatchType with (nolock) on
    MatchType.MatchingTypeID = Match.MatchingTypeID

where
    [User].UserID = coalesce(@UserID, [User].UserID)
  and
    [User].[Login] = coalesce(@Login, [User].[Login])
;

return;

end


GO
/****** Object:  UserDefinedFunction [napp_in].[get_agg_AvailableGroups_Name_ByProject]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.02.2020
-- Update date: 13.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_agg_AvailableGroups_Name_ByProject]
(
	@ProjectID int
	--,@MatchingID int = null
)
RETURNS nvarchar(max)
AS
begin 

	declare @GroupName_List dbo.StrList;

insert into @GroupName_List
(
    value
)
select
    GroupName
from
    --[napp_in].[get_AvailableGroups_ByProject] 
    --	(@ProjectID, @MatchingID) 
    dbo.Projects p with (nolock) 
		
		join dbo.Projects_Groups pg  with (nolock) on
    pg.ProjectID = p.ProjectID

    join dbo.[Groups] g with (nolock) on
    g.GroupID = pg.GroupID

where
    p.ProjectID = @ProjectID
;

return (select [dbo].[agg_StrList](@GroupName_List, ', ')) ;

end



GO
/****** Object:  UserDefinedFunction [napp_in].[get_agg_Technologies_NameRu_ByProject]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_agg_Technologies_NameRu_ByProject]
(
	@ProjectID int
	--,@MatchingID int = null
)
RETURNS nvarchar(max)
AS
begin 

	declare @TechnologyName_List dbo.StrList;

insert into @TechnologyName_List
(
    value
)
select
    tech.TechnologyName_ru
from
    dbo.Projects_Technologies p with (nolock)

		join dbo.Technologies tech  with (nolock) on
    tech.TechnologyID = p.TechnologyID
where
    p.ProjectID = @ProjectID
;

return (select [dbo].[agg_StrList](@TechnologyName_List, ', ')) ;

end




GO
/****** Object:  UserDefinedFunction [napp_in].[get_agg_Technologies_NameRu_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_agg_Technologies_NameRu_ByStudent]
(
	@StudentID int
)
RETURNS nvarchar(max)
AS
begin 

	declare @TechnologyName_List dbo.StrList;

insert into @TechnologyName_List
(
    value
)
select
    tech.TechnologyName_ru
from
    dbo.Students_Technologies s with (nolock)

		join dbo.Technologies tech  with (nolock) on
    tech.TechnologyID = s.TechnologyID
where
    s.StudentID = @StudentID
;

return (select [dbo].[agg_StrList](@TechnologyName_List, ', ')) ;

end





GO
/****** Object:  UserDefinedFunction [napp_in].[get_agg_WorkDirections_NameRu_ByProject]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_agg_WorkDirections_NameRu_ByProject]
(
	@ProjectID int
	--,@MatchingID int = null
)
RETURNS nvarchar(max)
AS
begin 

	declare @WorkDirectionName_List dbo.StrList;

insert into @WorkDirectionName_List
(
    value
)
select
    wd.DirectionName_ru
from
    dbo.Projects_WorkDirections p with (nolock)

		join dbo.WorkDirections wd  with (nolock) on
    wd.DirectionID = p.DirectionID
where
    p.ProjectID = @ProjectID
;

return (select [dbo].[agg_StrList](@WorkDirectionName_List, ', ')) ;

end




GO
/****** Object:  UserDefinedFunction [napp_in].[get_agg_WorkDirections_NameRu_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_agg_WorkDirections_NameRu_ByStudent]
(
	@StudentID int
	--,@MatchingID int = null
)
RETURNS nvarchar(max)
AS
begin 

	declare @WorkDirectionName_List dbo.StrList;

insert into @WorkDirectionName_List
(
    value
)
select
    wd.DirectionName_ru
from
    dbo.Students_WorkDirections s with (nolock)

		join dbo.WorkDirections wd  with (nolock) on
    wd.DirectionID = s.DirectionID
where
    s.StudentID = @StudentID
;

return (select [dbo].[agg_StrList](@WorkDirectionName_List, ', ')) ;

end





GO
/****** Object:  UserDefinedFunction [napp_in].[get_CurrentStageCode_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 13.02.2020
-- Update date: 
-- Description:	Возвращает номер этапа 
-- =============================================
CREATE FUNCTION [napp_in].[get_CurrentStageCode_ByMatching]
(
	@MatchingID int
)
RETURNS int
AS
BEGIN
	declare
@CurrentStageCode int
	; 

	set @CurrentStageCode = 
		(
			select top 1 
				StageType.StageTypeCode
			from 
				dbo.Stages Stage with (nolock)

				join dbo.StagesTypes StageType with (nolock) on 
					StageType.StageTypeID = Stage.StageTypeID

			where 
				Stage.MatchingID = @MatchingID
				and 
				Stage.IsCurrent = 1 -- true
		)
	; 

	if (@CurrentStageCode is not null)
		return @CurrentStageCode;

return -1;


END

GO
/****** Object:  UserDefinedFunction [napp_in].[get_CurrentStageID_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 19.02.2020
-- Update date: 
-- Description:	Возвращает ID этапа 
-- =============================================
CREATE FUNCTION [napp_in].[get_CurrentStageID_ByMatching]
(
	@MatchingID int
)
RETURNS int
AS
BEGIN
	declare
@CurrentStageID int
	; 

	set @CurrentStageID = 
		(
			select top 1 
				Stage.StageID
			from 
				dbo.Stages Stage with (nolock)

			where 
				Stage.MatchingID = @MatchingID
				and 
				Stage.IsCurrent = 1 -- true
		)
	; 

	if (@CurrentStageID is not null)
		return @CurrentStageID;

return -1;


END


GO
/****** Object:  View [dbo_v].[Students]    Script Date: 17.03.2024 12:16:12 ******/









-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 11.02.2020
-- Update date: 30.11.2020
-- Description:	
-- =============================================
CREATE view [dbo_v].[Students]
as
select
    Student.StudentID
     ,[Group].GroupID
        ,[Group].GroupName
        ,[User].UserID
        ,UserRole.MatchingID
        ,[User].Surname
        ,[User].Name
        ,[User].Patronimic
        ,[User].Surname
    + ' ' + iif ([User].Name is not null, substring([User].Name,1, 1) , '' ) + '.'
    + ' ' + iif ([User].Patronimic is not null, substring([User].Patronimic,1, 1) , '' ) + '.'
    as NameAbbreviation

        ,UserRole.LastVisitDate
        ,Student.Info
        ,Student.Info2
        ,[napp_in].[get_agg_WorkDirections_NameRu_ByStudent](Student.StudentID) as WorkDirectionsName_List
        ,[napp_in].[get_agg_Technologies_NameRu_ByStudent] (Student.StudentID) as TechnologiesName_List

from
    dbo.Students Student with (nolock)

    join dbo.Groups [Group] with (nolock) on
    [Group].GroupID = Student.GroupID

    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.StudentID = Student.StudentID

    join dbo.Users [User] (nolock) on
    [User].UserID = UserRole.UserID

    join dbo.Roles [Role] (nolock) on
    [Role].RoleID = UserRole.RoleID
    and
    [Role].RoleType = 1 -- Тип роли = бизнес роль 
    and
    [Role].RoleCode = 2 -- Роль = студент










    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage4_Students]    Script Date: 17.03.2024 12:16:12 ******/
    CREATE FUNCTION [napp].[get_StatisticStage4_Students]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Student.StudentID						AS StudentID
     ,Student.Name							AS StudentName
     ,Student.Surname						AS StudentSurname
     ,Student.Patronimic						AS StudentPatronimic
     ,Student.NameAbbreviation				AS StudentNameAbbreviation
     ,Student.LastVisitDate					AS StudentLastVisitDate
     ,Student.GroupID						AS StudentGroupID
     ,Student.GroupName						AS StudentGroupName

     ,iif(Project.IsClosed = 1, 1, 0) as ProjectIsAllocated
     ,iif(Choice.IsInQuota = 1, 1, 0) as ChoiceIsInQuota
     ,iif(Choice.IsInQuota = 0, 1, 0) as ChoiceIsOutQuota
     ,iif(Choice.ChoiceID is null, 1, 0) as ChoiceIsCantAllocated

     ,PreferenceType.TypeName_ru				AS PreferenceTypeName_ru

from
    dbo_v.Students Student with (nolock)

		left join dbo.TutorsChoice Choice with (nolock) on
    Choice.StudentID = Student.StudentID
    and
    Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID)

    left join dbo.Projects Project with (nolock) on
    Project.ProjectID = Choice.ProjectID

    left join dbo.StudentsPreferences Preference with (nolock) on
    Preference.PreferenceID = Choice.PreferenceID

    left join dbo.ChoosingTypes PreferenceType with (nolock) on
    PreferenceType.TypeID = Preference.TypeID
where
    Student.MatchingID = @MatchingID
    GO
/****** Object:  View [dbo_v].[CommonQuotas]    Script Date: 17.03.2024 12:16:12 ******/



CREATE view [dbo_v].[CommonQuotas]
as
select
    Tutor.MatchingID
     ,Tutor.TutorID
     ,Tutor.IsReadyToStart
     --,TutorUser.Name
     --,TutorUser.Surname
     --,TutorUser.Patronimic
     --,TutorUser.Surname 
     --		+ ' ' + iif (TutorUser.Name is not null, substring(TutorUser.Name,1, 1) , '' ) + '.'
     --		+ ' ' + iif (TutorUser.Patronimic is not null, substring(TutorUser.Patronimic,1, 1) , '' ) + '.'
     --			as NameAbbreviation  
     ,CommonQuota.CommonQuotaID
     ,CommonQuota.Qty
     ,CommonQuota.CreateDate
     ,CommonQuota.QuotaStateID
     ,CommonQuota.UpdateDate
     ,CommonQuota.IsNotification
     ,CommonQuota.Message
     ,CommonQuota.StageID

     ,Stage.IsCurrent
     ,StageType.StageTypeCode
     ,StageType.StageTypeName
     ,StageType.StageTypeName_ru
     ,Stage.IterationNumber

     ,QuotasState.QuotaStateCode
     ,QuotasState.QuotaStateName
     ,QuotasState.QuotaStateName_ru


from  dbo.CommonQuotas CommonQuota with (nolock)  

	join dbo.Tutors Tutor with (nolock) on
    Tutor.TutorID = CommonQuota.TutorID

    join dbo.Stages [Stage] with (nolock) on
    [Stage].StageID = CommonQuota.StageID

    join dbo.StagesTypes StageType with (nolock) on
    StageType.StageTypeID = [Stage].StageTypeID

    join dbo.QuotasStates QuotasState with (nolock) on
    QuotasState.QuotaStateID = CommonQuota.QuotaStateID

    --join dbo.Users_Roles TutorUserRole with (nolock) on  
    --	TutorUserRole.TutorID = Tutor.TutorID 
    --	and 
    --	TutorUserRole.MatchingID = Tutor.MatchingID 

    --join dbo.Roles TutorRole with (nolock) on
    --	TutorUserRole.RoleID = TutorRole.RoleID 
    --	and
    --	TutorRole.RoleCode = 1 	--Tutor

    --join dbo.Users TutorUser with(nolock) on
    --	TutorUserRole.UserID = TutorUser.UserID






    GO
/****** Object:  View [dbo_v].[Tutors]    Script Date: 17.03.2024 12:16:12 ******/
    




-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 12.02.2020
-- Update date: 07.03.2020
-- Description:	
-- =============================================
CREATE view [dbo_v].[Tutors]
as
select
    Tutor.TutorID
     ,Tutor.IsClosed
     ,Tutor.IsReadyToStart
     ,[User].UserID
     ,UserRole.MatchingID
     ,[User].Surname
     ,[User].Name
     ,[User].Patronimic
     ,UserRole.LastVisitDate

     ,[User].Surname
    + ' ' + iif ([User].Name is not null, substring([User].Name,1, 1) , '' ) + '.'
    + ' ' + iif ([User].Patronimic is not null, substring([User].Patronimic,1, 1) , '' ) + '.'
    as NameAbbreviation

from
    dbo.Tutors Tutor with (nolock)

    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.TutorID = Tutor.TutorID

    join dbo.Users [User] (nolock) on
    [User].UserID = UserRole.UserID





    GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_Requests_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/
    -- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 22.01.2020
-- Update date: 14.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_Requests_ByExecutive]
(	
	@UserID int
	,@MatchingID int
)
returns table 
as
return

select
    Quota.CommonQuotaID as QuotaID
     ,Quota.TutorID
     ,Tutor.Name
     ,Tutor.Surname
     ,Tutor.Patronimic
     ,Tutor.NameAbbreviation
     ,Quota.Qty as RequestedQuotaQty
     ,Quota.Message
     ,napp.get_CommonQuota_ByTutor(Quota.TutorID) as CurrentQuotaQty
     ,Quota.IsNotification
     ,Quota.CreateDate
     ,Quota.UpdateDate

from
    dbo_v.CommonQuotas Quota with (nolock) 

	join dbo_v.Tutors Tutor with (nolock) on
    Tutor.TutorID = Quota.TutorID

    join Users_Roles ExecutiveUserRole with (nolock) on
    ExecutiveUserRole.MatchingID = Quota.MatchingID


where
    Quota.QuotaStateName = 'requested'
  and
    ExecutiveUserRole.MatchingID = @MatchingID
  and
    ExecutiveUserRole.UserID = @UserID
  and
    ExecutiveUserRole.RoleID = (	select RoleID
    from
    dbo.Roles with(nolock)
    where
    RoleCode =  3 )--execuеtive


    GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_History_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 22.02.2020
-- Update date: 13.03.2020
-- Description:	История запросов общей квоты по преподавателю
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_History_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
select
    Quota.CommonQuotaID as QuotaID
     ,Quota.CreateDate
     ,Quota.UpdateDate
     ,Quota.Qty
     ,Quota.Message
     ,Quota.IsNotification
     ,QuotaState.QuotaStateCode
     ,QuotaState.QuotaStateName
     ,QuotaState.QuotaStateName_ru
     ,Stage.StageID
     ,Stage.StageName
     ,StageType.StageTypeCode
     ,StageType.StageTypeName
     ,StageType.StageTypeName_ru
     ,Stage.IterationNumber

from
    dbo.CommonQuotas Quota with (nolock)

		join dbo.QuotasStates QuotaState with (nolock) on
    QuotaState.QuotaStateID = Quota.QuotaStateID

    join dbo.Stages [Stage] with (nolock) on
    [Stage].StageID = Quota.StageID

    join dbo.StagesTypes StageType with (nolock) on
    StageType.StageTypeID = [Stage].StageTypeID

where
    Quota.TutorID = @TutorID






    GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_Request_Notification_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 24.02.2020
-- Update date: 04.04.2020
-- Description:	Выводит уведомление для преподавателя, если оно есть 
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_Request_Notification_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
select
    Quota.QuotaID
     ,Quota.Qty
     ,Quota.QuotaStateCode
     ,Quota.QuotaStateName
     ,Quota.QuotaStateName_ru

     ,Quota.CreateDate
     ,Quota.UpdateDate

from
    [napp].[get_CommonQuota_History_ByTutor]( @TutorID) Quota

where
    Quota.IsNotification = 1
  and
--Quota.QuotaStateName = 'requested'
    Quota.QuotaStateName in ('active', 'declined')






    GO
/****** Object:  View [dbo_v].[Projects]    Script Date: 17.03.2024 12:16:12 ******/









CREATE view [dbo_v].[Projects]
as

select
    Project.TutorID
     ,Tutor.UserID
     ,Tutor.Surname		as TutorSurname
     ,Tutor.Name			as TutorName
     ,Tutor.Patronimic	as TutorPatronimic
     ,Tutor.NameAbbreviation as TutorNameAbbreviation
     ,Tutor.IsClosed		as Tutor_IsClosed

     ,Project.ProjectID
--,iif (Project.IsDefault = 1, 'Записаться к преподавателю' ,Project.ProjectName) as ProjectName
     ,Project.ProjectName as ProjectName
     ,Project.Info
     ,Project.IsClosed
     ,Project.IsDefault

     ,Project.MatchingID

--,ProjectQuota.Qty as Qty
     ,Project.ProjectQuotaQty as Qty
     ,iif(	Project.ProjectQuotaQty is not null
    ,cast(Project.ProjectQuotaQty as nvarchar (50))
    ,'Не важно') as QtyDescription

     ,[napp_in].[get_agg_AvailableGroups_Name_ByProject] (Project.ProjectID) as AvailableGroupsName_List
     ,[napp_in].[get_agg_Technologies_NameRu_ByProject] (Project.ProjectID) as TechnologiesName_List
     ,[napp_in].[get_agg_WorkDirections_NameRu_ByProject] (Project.ProjectID) as WorkDirectionsName_List


from
    dbo.Projects Project with (nolock)

    join dbo_v.Tutors Tutor on
    Tutor.TutorID = Project.TutorID

--join dbo_v.ActiveQuotas ProjectQuota on 
--	 ProjectQuota.ProjectID = Project.ProjectID
--	 and 
--	 ProjectQuota.GroupID is null 
--	 and 
--	 ProjectQuota.IsCommon = 0 -- false
;









GO
/****** Object:  UserDefinedFunction [napp].[get_CurrentStage_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 13.02.2020
-- Update date: 14.05.2020
-- Description:	Описание текущего состояния распределения. 
--				Название и код этапа, дата начала и плановая дата окончания  этапа, номер итерации, это этап  Итераций
--				Название распределения
-- =============================================
CREATE FUNCTION [napp].[get_CurrentStage_ByMatching]
(
	@MatchingID int
)
RETURNS table 
as 
return

select top 1
		Stage.StageID		
		,Stage.StageName
     ,Stage.StartDate
     ,Stage.EndPlanDate
     ,Stage.IterationNumber

     ,StagesType.StageTypeID
     ,StagesType.StageTypeCode
     ,StagesType.StageTypeName
     ,StagesType.StageTypeName_ru

     ,Matching.MatchingID
     ,Matching.MatchingName


from
    dbo.Matching Matching with (nolock) 

		join dbo.Stages Stage with (nolock) on
    Stage.MatchingID = Matching.MatchingID

    join dbo.StagesTypes StagesType with (nolock) on
    StagesType.StageTypeID = Stage.StageTypeID

where
    Matching.MatchingID = @MatchingID
  and
    Stage.IsCurrent = 1 -- true



    GO
/****** Object:  View [dbo_v].[Allocation]    Script Date: 17.03.2024 12:16:12 ******/

CREATE view [dbo_v].[Allocation]
as
select
    MatchingStage.MatchingID
     ,MatchingStage.StageTypeID
     ,MatchingStage.StageTypeCode
     ,MatchingStage.StageTypeName
     ,MatchingStage.StageTypeName_ru
     ,Student.StudentID
     ,Student.GroupID
     ,TutorsChoice.ChoiceID
     ,TutorsChoice.ProjectID
     ,TutorsChoice.SortOrderNumber
     ,TutorsChoice.PreferenceID
     ,iif(TutorsChoice.ChoiceID is not null, 1, 0) as IsAllocated
     ,ChoosingType.TypeID
     ,ChoosingType.TypeCode
     ,ChoosingType.TypeName
     ,ChoosingType.TypeName_ru

from
    dbo.Students as Student with (nolock) 

	cross apply 
	(
		select 
			* 
		from 
			napp.get_CurrentStage_ByMatching (Student.MatchingID) Stage 
		
	) as MatchingStage

	left join dbo.TutorsChoice as TutorsChoice with (nolock) on
    TutorsChoice.StudentID = Student.StudentID
    and
    TutorsChoice.StageID = MatchingStage.StageID
    and
    TutorsChoice.IsInQuota = 1	-- Убрано условие по этапу и добавлено это, чтобы на каждой интерацц смотреть кто распределен. 
    -- Дополнительно нужно раскашивать признаком закрыт или нет проект

    left join dbo.ChoosingTypes as ChoosingType with (nolock) on
    ChoosingType.TypeID = TutorsChoice.TypeID



    --where 
    --MatchingStage.StageTypeCode in (5, 6)


    --left join dbo.TutorsChoice TutorsChoice with (nolock) 
    GO
/****** Object:  View [dbo_v].[Allocation_FullInfo]    Script Date: 17.03.2024 12:16:12 ******/
    CREATE view [dbo_v].[Allocation_FullInfo]
as
select
    Allocation.MatchingID
     ,Allocation.StageTypeID
     ,Allocation.StageTypeCode
     ,Allocation.StageTypeName
     ,Allocation.StageTypeName_ru

     ,Student.StudentID
     ,Student.[Name]				as StudentName
     ,Student.Surname			as StudentSurname
     ,Student.Patronimic			as StudentPatronimic
     ,Student.NameAbbreviation	as StudentNameAbbreviation
     ,Student.GroupID
     ,Student.GroupName

     ,Allocation.IsAllocated
     ,Allocation.ChoiceID
     ,Allocation.PreferenceID
     ,Allocation.SortOrderNumber

     ,Project.ProjectID
     ,Project.ProjectName
     ,Project.TutorID
     ,Project.TutorName
     ,Project.TutorSurname
     ,Project.TutorPatronimic
     ,Project.TutorNameAbbreviation

     ,Allocation.TypeID
     ,Allocation.TypeCode
     ,Allocation.TypeName
     ,Allocation.TypeName_ru



from
    dbo_v.Allocation as Allocation with(nolock) 

	left join dbo_v.Students as Student  with(nolock)  on
    Student.StudentID =   Allocation.StudentID

    left join dbo_v.Projects as Project with(nolock)  on
    Project.ProjectID = Allocation.ProjectID

    GO
/****** Object:  UserDefinedFunction [napp].[get_FinishedAllocations]    Script Date: 17.03.2024 12:16:12 ******/
    
-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 25.04.2020
-- Update date: 31.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_FinishedAllocations] ()
RETURNS  TABLE 
as
return
select
    Allocation.MatchingID
     ,Allocation.StageTypeID
     ,Allocation.StageTypeCode
     ,Allocation.StageTypeName
     ,Allocation.StageTypeName_ru

     ,Allocation.StudentID
     ,Allocation.StudentName
     ,Allocation.StudentSurname
     ,Allocation.StudentPatronimic
     ,Allocation.StudentNameAbbreviation
     ,Allocation.GroupID
     ,Allocation.GroupName

     ,cast(Allocation.IsAllocated as bit) as IsAllocated
     ,Allocation.ChoiceID
     ,Allocation.PreferenceID
     ,Allocation.SortOrderNumber

     ,Allocation.ProjectID
     ,Allocation.ProjectName
     ,Allocation.TutorID
     ,Allocation.TutorName
     ,Allocation.TutorSurname
     ,Allocation.TutorPatronimic
     ,Allocation.TutorNameAbbreviation

--,Allocation.TypeID
--,Allocation.TypeCode
--,Allocation.TypeName
--,Allocation.TypeName_ru
from
    dbo_v.Allocation_FullInfo as Allocation with (nolock)
where
    Allocation.StageTypeCode = 6



    GO
/****** Object:  UserDefinedFunction [napp].[get_Allocation_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 25.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Allocation_ByExecutive]
(	
	@UserID int
	,@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Allocation.MatchingID
     ,Allocation.StageTypeID
     ,Allocation.StageTypeCode
     ,Allocation.StageTypeName
     ,Allocation.StageTypeName_ru

     ,Allocation.StudentID
     ,Allocation.StudentName
     ,Allocation.StudentSurname
     ,Allocation.StudentPatronimic
     ,Allocation.StudentNameAbbreviation
     ,Allocation.GroupID
     ,Allocation.GroupName

     ,cast(Allocation.IsAllocated as bit) as IsAllocated
     ,Allocation.ChoiceID
     ,Allocation.PreferenceID
     ,Allocation.SortOrderNumber

     ,Allocation.ProjectID
     ,Allocation.ProjectName
     ,Allocation.TutorID
     ,Allocation.TutorName
     ,Allocation.TutorSurname
     ,Allocation.TutorPatronimic
     ,Allocation.TutorNameAbbreviation

     ,Allocation.TypeID
     ,Allocation.TypeCode
     ,Allocation.TypeName
     ,Allocation.TypeName_ru
from
    dbo_v.Allocation_FullInfo as Allocation with (nolock)	

		join Users_Roles ExecutiveUserRole with (nolock) on
    ExecutiveUserRole.MatchingID = Allocation.MatchingID

where
    Allocation.MatchingID = @MatchingID
  and
    ExecutiveUserRole.UserID = @UserID
  and
    ExecutiveUserRole.RoleID = (	select RoleID
    from
    dbo.Roles with(nolock)
    where
    RoleCode =  3 )--execuеtive


    GO
/****** Object:  UserDefinedFunction [napp].[get_FinishedAllocations_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekatherina
-- Create date: 31.05.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_FinishedAllocations_ByMatching] 
(
	@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Allocation.MatchingID
     ,Allocation.StageTypeID
     ,Allocation.StageTypeCode
     ,Allocation.StageTypeName
     ,Allocation.StageTypeName_ru

     ,Allocation.StudentID
     ,Allocation.StudentName
     ,Allocation.StudentSurname
     ,Allocation.StudentPatronimic
     ,Allocation.StudentNameAbbreviation
     ,Allocation.GroupID
     ,Allocation.GroupName

     ,cast(Allocation.IsAllocated as bit) as IsAllocated
     ,Allocation.ChoiceID
     ,Allocation.PreferenceID
     ,Allocation.SortOrderNumber

     ,Allocation.ProjectID
     ,Allocation.ProjectName
     ,Allocation.TutorID
     ,Allocation.TutorName
     ,Allocation.TutorSurname
     ,Allocation.TutorPatronimic
     ,Allocation.TutorNameAbbreviation

--,Allocation.TypeID
--,Allocation.TypeCode
--,Allocation.TypeName
--,Allocation.TypeName_ru
from
    dbo_v.Allocation_FullInfo as Allocation with (nolock)
where
    Allocation.StageTypeCode = 6
  and
    Allocation.MatchingID = @MatchingID




    GO
/****** Object:  UserDefinedFunction [napp].[get_CommonQuota_History_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 14.03.2020
-- Update date: 05.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_CommonQuota_History_ByExecutive]
(	
	@UserID int
	,@MatchingID int
)
returns table 
as
return

select
    Quota.CommonQuotaID as QuotaID
     ,Quota.TutorID
     ,Tutor.Name
     ,Tutor.Surname
     ,Tutor.Patronimic
     ,Tutor.NameAbbreviation
     ,Quota.Qty as RequestedQuotaQty
     ,Quota.Message
     ,napp.get_CommonQuota_ByTutor(Quota.TutorID) as CurrentQuotaQty
     ,Quota.IsNotification

     ,Quota.QuotaStateCode
     ,Quota.QuotaStateName
     ,Quota.QuotaStateName_ru
     ,Quota.UpdateDate
     ,Quota.CreateDate

     ,Quota.StageID
     ,Quota.StageTypeCode
     ,Quota.StageTypeName
     ,Quota.StageTypeName_ru
     ,Quota.IterationNumber

from
    dbo_v.CommonQuotas Quota with (nolock) 

	join dbo_v.Tutors Tutor with (nolock) on
    Tutor.TutorID = Quota.TutorID

    join Users_Roles ExecutiveUserRole with (nolock) on
    ExecutiveUserRole.MatchingID = Quota.MatchingID



where
--Quota.QuotaStateName = 'requested'
--and
    ExecutiveUserRole.MatchingID = @MatchingID
  and
    ExecutiveUserRole.UserID = @UserID
  and
    ExecutiveUserRole.RoleID = (	select RoleID
    from
    dbo.Roles with(nolock)
    where
    RoleCode =  3 )--execuеtive



    GO
/****** Object:  UserDefinedFunction [napp].[get_Tutors_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 28.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Tutors_ByMatching]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Tutor.TutorID
     ,Tutor.[Name] 				as TutorName
     ,Tutor.Surname				as TutorSurname
     ,Tutor.Patronimic			as TutorPatronimic
     ,Tutor.NameAbbreviation 	as TutorNameAbbreviation

     ,Project.ProjectID
     --,iif (Project.IsDefault = 1, 'Записаться к преподавателю' ,Project.ProjectName) as ProjectName
     ,Project.ProjectName	as ProjectName

from
    dbo.Projects Project with (nolock)

		join dbo_v.Tutors Tutor on
    Tutor.TutorID = Project.TutorID

where
    Tutor.MatchingID = @MatchingID
  and
    Project.IsDefault = 1



    GO
/****** Object:  UserDefinedFunction [napp].[get_FinishedMatchings]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 28.04.2020
-- Update date: 
-- Description:	
-- =============================================
create FUNCTION [napp].[get_FinishedMatchings]
()
RETURNS  TABLE 
as
return
select
    Matching.MatchingID
     ,Matching.MatchingName

from
    dbo.Matching Matching with (nolock) 

		cross apply
		(
			select 
				* 
			from 
				napp.get_CurrentStage_ByMatching(Matching.MatchingID)
		) as CurrentStage

where
    CurrentStage.StageTypeCode = 6


    GO
/****** Object:  UserDefinedFunction [napp].[get_AllocatedProject_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 28.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_AllocatedProject_ByStudent]
(
	@StudentID int
)
RETURNS  TABLE 
as
return
select
    Allocation.StudentID

     ,Project.TutorID
     ,Project.TutorName
     ,Project.TutorSurname
     ,Project.TutorPatronimic
     ,Project.TutorNameAbbreviation

     ,Project.ProjectID
     ,Project.ProjectName


from
    dbo_v.Allocation as Allocation with(nolock) 

		left join dbo_v.Projects as Project with(nolock)  on
    Project.ProjectID = Allocation.ProjectID

where
    StudentID = @StudentID
  and
    Allocation.StageTypeCode = 6


    GO
/****** Object:  UserDefinedFunction [napp].[get_StudentInfo]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 30.11.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StudentInfo]
(	
	@StudentID int
)
RETURNS  TABLE 
as
return
SELECT
    Student.[StudentID]
     ,Student.[GroupID]
     ,Student.[GroupName]
     ,Student.[UserID]
     ,Student.[MatchingID]
     ,Student.[Surname]
     ,Student.[Name]
     ,Student.[Patronimic]
     ,Student.[LastVisitDate]
     ,Student.[Info]
     ,Student.[Info2]
     ,Student.[WorkDirectionsName_List]
     ,Student.[TechnologiesName_List]


FROM
    [dbo_v].[Students] Student


where
    @StudentID is not null
  and
    Student.StudentID = @StudentID

    GO
/****** Object:  UserDefinedFunction [napp].[get_Projects_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 26.11.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Projects_ByStudent]
(	
	@StudentID int
)
RETURNS  TABLE 
as
return
SELECT
    Project.[TutorID]
     ,Project.TutorNameAbbreviation
     ,Project.[UserID]
     ,Project.[TutorSurname]
     ,Project.[TutorName]
     ,Project.[TutorPatronimic]


     ,Project.[ProjectID]
     ,Project.[ProjectName]
     --,iif (Project.IsDefault = 1, 'Записаться к преподавателю' ,Project.ProjectName) as ProjectName
     ,Project.[Info]

     ,Project.[TechnologiesName_List]
     ,Project.[WorkDirectionsName_List]

     ,Student.StudentID
     ,Student.GroupID

     ,cast (iif (Preference.PreferenceID is not null, 1, 0) as bit) as IsSelectedByStudent
     ,Preference.PreferenceID
     ,Preference.OrderNumber

FROM
    [dbo_v].[Projects] Project with (nolock)

    cross apply
    (
    select
    StudentID
        ,GroupID
    from
    dbo.Students Student_in with (nolock)
    where
    Student_in.StudentID = @StudentID
    ) as Student

    join dbo.Projects_Groups ProjectGroup with (nolock) on
    ProjectGroup.ProjectID = Project.ProjectID
    and
    ProjectGroup.GroupID = Student.GroupID


    left join dbo.StudentsPreferences Preference with (nolock) on
    Preference.StudentID = Student.StudentID
    and
    Preference.ProjectID = Project.ProjectID




where
    @StudentID is not null
  and
    coalesce(Project.Qty, -1) != 0
--and 
--not exists (
--				select 1 
--				from dbo_v.ActiveQuotas Quota 
--				where 
--					Quota.ProjectID = Project.ProjectID 
--					and 
--					Quota.IsCommon = 0 
--					and 
--					Quota.Qty = 0 
--					and 
--					Quota.GroupID = Student.GroupID )




    GO
/****** Object:  UserDefinedFunction [napp].[get_Projects_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 11.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Projects_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
SELECT --[TutorID]
    --,[UserID]
    -- ,[TutorSurname]
    -- ,[TutorName]
    -- ,[TutorPatronimic]
    [ProjectID]
        ,[ProjectName]
        ,[Info]
        ,[IsClosed]
        ,Qty
        , QtyDescription
        ,[AvailableGroupsName_List]
        ,[TechnologiesName_List]
        ,[WorkDirectionsName_List]
        ,IsDefault
FROM [dbo_v].[Projects]
where
    @TutorID is not null
  and
    TutorID = @TutorID



    GO
/****** Object:  UserDefinedFunction [napp].[get_TutorsChoice_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/
-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 21.03.2020
-- Update date: 25.04.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_TutorsChoice_ByTutor]
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
    GO
/****** Object:  View [dbo_v].[ActiveCommonQuotas]    Script Date: 17.03.2024 12:16:12 ******/



-- Возвращает действительную общую квот преподавателей 
CREATE view [dbo_v].[ActiveCommonQuotas]
as
select
    Quota.*
from
    dbo.CommonQuotas Quota with (nolock)

	join dbo.QuotasStates QuotaState with (nolock) on
    QuotaState.QuotaStateID = Quota.QuotaStateID

where
    QuotaState.QuotaStateCode = 1 --active



    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage2_Tutors]    Script Date: 17.03.2024 12:16:12 ******/
create FUNCTION [napp].[get_StatisticStage2_Tutors]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
SELECT
    Tutor.TutorID				as TutorID
     ,Tutor.Name					as TutorName
     ,Tutor.Surname				as TutorSurname
     ,Tutor.Patronimic			as TutorPatronimic
     ,Tutor.NameAbbreviation		as TutorNameAbbreviation
     ,Tutor.IsReadyToStart		as TutorIsReadyToStart
     ,Quota.Qty					as QuotaQty
     ,Tutor.LastVisitDate		as TutorLastVisitDate
     ,count (Project.ProjectID)	as ProjectsCount

FROM
    DiplomaMatching.dbo_v.Tutors Tutor with (nolock)


		join dbo_v.ActiveCommonQuotas Quota with (nolock) on
    Quota.TutorID = Tutor.TutorID

    left join dbo.Projects Project with (nolock) on
    Project.TutorID = Tutor.TutorID

where
    @MatchingID is not null
  and
    Tutor.MatchingID = @MatchingID

group by
    Tutor.TutorID
        ,Tutor.Name
        ,Tutor.Surname
        ,Tutor.Patronimic
        ,Tutor.NameAbbreviation
        ,Tutor.IsReadyToStart
        ,Quota.Qty
        ,Tutor.LastVisitDate


    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage2_Tutor_Projects]    Script Date: 17.03.2024 12:16:12 ******/
CREATE function [napp].[get_StatisticStage2_Tutor_Projects]
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
    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Tutors]    Script Date: 17.03.2024 12:16:12 ******/
create function [napp].[get_StatisticStage3_Tutors](
@MatchingID int
)
RETURNS TABLE
as
return
SELECT
    *
FROM
    napp.get_StatisticStage2_Tutors(@MatchingID)

    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Tutor_Projects]    Script Date: 17.03.2024 12:16:12 ******/
create function [napp].[get_StatisticStage3_Tutor_Projects]
(
@MatchingID int
,@TutorID int
)
RETURNS TABLE
as
return
SELECT
    *
FROM
    napp.get_StatisticStage2_Tutor_Projects(@MatchingID,@TutorID)

    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Students]    Script Date: 17.03.2024 12:16:12 ******/
CREATE FUNCTION [napp].[get_StatisticStage3_Students]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return

SELECT
    Student.StudentID				as StudentID
     ,Student.Name					as StudentName
     ,Student.Surname				as StudentSurname
     ,Student.Patronimic				as StudentPatronimic
     ,Student.NameAbbreviation		as StudentNameAbbreviation
     ,Student.LastVisitDate			as StudentLastVisitDate

     ,0 as IsSetInfo
     ,iif(count(Technology.StudentTechnologyID) > 0, 1, 0) as IsSetTechnologies
     ,iif(count(Direction.StudentDirectionID) > 0, 1, 0) as IsSetWorkDirections

     ,[Group].GroupName				as StudentGroupName

        ,(case
    when count(DISTINCT Preference.PreferenceID) != 0 then count( DISTINCT Preference.PreferenceID)
    else '-'
    end
    )								as ProjectCount


FROM
    dbo_v.Students Student with (nolock)

    join dbo.Groups [Group] with (nolock) on
    [Group].GroupID = Student.GroupID

    --join dbo_v.Users_FullInfo [User] with (nolock) on 
    --	[User].StudentID = Student.StudentID
    --	and 
    --	[User].MatchingID = @MatchingID
    --	and 
    --	[User].RoleCode = 2

    left join dbo.StudentsPreferences Preference with (nolock) on
    Preference.StudentID = Student.StudentID

    left join dbo.Students_Technologies Technology with (nolock) on
    Technology.StudentID = Student.StudentID

    left join dbo.Students_WorkDirections Direction with (nolock) on
    Direction.StudentID = Student.StudentID

where
    @MatchingID is not null
  and
    Student.MatchingID = @MatchingID
group by
    Student.StudentID
        ,Student.Name
        ,Student.Surname
        ,Student.Patronimic
        ,Student.NameAbbreviation
        ,Student.LastVisitDate

        ,[Group].GroupName
    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage3_Student_Projects]    Script Date: 17.03.2024 12:16:12 ******/
CREATE function [napp].[get_StatisticStage3_Student_Projects]
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

    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage4_Tutors]    Script Date: 17.03.2024 12:16:12 ******/
create FUNCTION [napp].[get_StatisticStage4_Tutors]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Tutor.TutorID				AS TutorID
     ,Tutor.Name					AS TutorName
     ,Tutor.Surname				AS TutorSurname
     ,Tutor.Patronimic			AS TutorPatronimic
     ,Tutor.NameAbbreviation		AS TutorNameAbbreviation
     ,Quota.Qty					AS TutorQuotaQty
     ,Tutor.LastVisitDate		AS TutorLastVisitDate

     ,count(distinct Project.ProjectID)  as TutorProjectsAllCount		-- ���������� ��������
     ,count(distinct case when Project.IsClosed = 1 then Project.ProjectID end)  as TutorProjectsClosedCount -- �������� �������

     ,iif (count(case when CType.TypeName = 'Self' then Choice.ChoiceID end) > 0, 1, 0) as TutorIsSelfChoice --��������������� �� �����
     ,iif (count(case when Choice.IsInQuota = 0 then Choice.ChoiceID end) > 0, 1, 0) as TutorIsAvailableChoice--���� �� �����

     ,count(case when Choice.IsInQuota = 1 then Choice.ChoiceID end) as TutorStudentsInQuotaCount -- ��������� � �����
     ,count(case when Choice.IsInQuota = 0 then Choice.ChoiceID end) as TutorStudentsOutQuotaCount-- ��������� �� � ����� 


from
    dbo_v.Tutors Tutor with (nolock) 

		join dbo_v.ActiveCommonQuotas Quota with (nolock) on
    Quota.TutorID = Tutor.TutorID

    left join dbo.Projects Project with (nolock) on
    Project.TutorID = Tutor.TutorID

    left join dbo.TutorsChoice Choice with (nolock) on
    Choice.ProjectID = Project.ProjectID
    and
    Choice.StageID = napp_in.get_CurrentStageID_ByMatching (@MatchingID)

    left join dbo.ChoosingTypes CType with (nolock) on
    CType.TypeID = Choice.TypeID

where
    Tutor.MatchingID = @MatchingID

group by
    Tutor.TutorID
        ,Tutor.Name
        ,Tutor.Surname
        ,Tutor.Patronimic
        ,Tutor.NameAbbreviation
        ,Tutor.IsReadyToStart
        ,Quota.Qty
        ,Tutor.LastVisitDate
    GO
/****** Object:  UserDefinedFunction [napp].[get_StatisticStage4_Tutors_Project_Allocated]    Script Date: 17.03.2024 12:16:12 ******/
CREATE FUNCTION [napp].[get_StatisticStage4_Tutors_Project_Allocated]
(	
	@MatchingID int,
	@TutorID int
)
RETURNS  TABLE 
as
return
SELECT
    StudentNameAbbreviation		AS Allocation_FullInfoStudentNameAbbreviation
     --,StudentID						AS Allocation_FullInfoStudentID
     --,ProjectID						AS Allocation_FullInfoProjectID
     ,ProjectName					AS Allocation_FullInfoProjectName
     ,TypeName_ru					AS Allocation_FullInfoTypeName_ru


from dbo_v.Allocation_FullInfo with(nolock)
WHERE dbo_v.Allocation_FullInfo.MatchingID = @MatchingID
  AND dbo_v.Allocation_FullInfo.TutorID = @TutorID

    GO
/****** Object:  UserDefinedFunction [napp].[get_Groups_All_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Groups_All_ByMatching]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
SELECT
    [Group].GroupID as GroupID
        ,[Group].GroupName as GroupName

FROM
    dbo.Groups [Group] with (nolock)

where
    [Group].MatchingID = @MatchingID

    GO
/****** Object:  UserDefinedFunction [napp].[get_Groups_ByTutor]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 14.05.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Groups_ByTutor]
(	
	@TutorID int
)
RETURNS  TABLE 
as
return
SELECT
    [Group].GroupID as GroupID
        ,[Group].GroupName as GroupName

FROM
    dbo.Groups [Group] with (nolock)

    join dbo.Tutors_Groups TutorGroup with (nolock) on
    TutorGroup.GroupID = [Group].GroupID

where
    TutorGroup.TutorID = @TutorID


    GO
/****** Object:  UserDefinedFunction [napp].[get_Groups_WithSelected_ByProject]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 14.05.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Groups_WithSelected_ByProject]
(	
	@ProjectID int
)
RETURNS  TABLE 
as
return
SELECT
    g.GroupID as GroupID
     ,g.GroupName as GroupName
     ,cast (iif(pg.GroupID is null, 0, 1) as bit) as IsSelectedByProject

FROM
    dbo.Groups g with (nolock)

		join dbo.Tutors_Groups tg with (nolock) on
    tg.GroupID = g.GroupID
    and
    tg.TutorID = (select top 1 p.TutorID from dbo.Projects p with (nolock) where p.ProjectID = @ProjectID)

    left join dbo.Projects_Groups pg with (nolock) on
    pg.GroupID = g.GroupID
    and
    pg.ProjectID = @ProjectID

where
    g.MatchingID = (select top 1
    p.MatchingID
    from
    dbo.Projects p with (nolock)
    where
    p.ProjectID = @ProjectID)




    GO
/****** Object:  UserDefinedFunction [napp].[get_Matchings]    Script Date: 17.03.2024 12:16:12 ******/
-- =============================================
-- Author:		Alexandr Filonov
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Matchings]
(	
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT
	m.MatchingID,
	m.MatchingName,
	m.CreatorUserID,
	m.MatchingTypeID,
	mt.MatchingTypeCode,
	mt.MatchingTypeName,
	mt.MatchingTypeName_ru
	
	FROM dbo.Matching m 
	JOIN dbo.MatchingType mt on m.MatchingTypeID = mt.MatchingTypeID
)


GO
/****** Object:  UserDefinedFunction [napp].[get_ProjectBasicInfo]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 14.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_ProjectBasicInfo]
(	
	@ProjectID int
)
RETURNS  TABLE 
as
return
SELECT --[TutorID]
     --,[UserID]
     -- ,[TutorSurname]
     -- ,[TutorName]
     -- ,[TutorPatronimic]
    Project.[ProjectID]
     --,iif (Project.IsDefault = 1, 'Записаться к преподавателю' ,Project.ProjectName) as ProjectName
     ,Project.ProjectName as ProjectName
     ,Project.[Info]
     ,Project.[IsClosed]

     ,Project.ProjectQuotaQty as Qty
     ,iif(	Project.ProjectQuotaQty is not null
    ,cast(Project.ProjectQuotaQty as nvarchar (50))
    ,'Не важно') as QtyDescription

FROM
    [DiplomaMatching].dbo.Projects Project with (nolock)

--join dbo_v.ActiveQuotas ProjectQuota on 
--	 ProjectQuota.ProjectID = Project.ProjectID
--	 and 
--	 ProjectQuota.GroupID is null 
--	 and 
--	 ProjectQuota.IsCommon = 0 -- false

where
    @ProjectID is not null
  and
    Project.ProjectID = @ProjectID


    GO
/****** Object:  UserDefinedFunction [napp].[get_Stages_History_ByExecutive]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 28.04.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Stages_History_ByExecutive]
(	
	@UserID int
	,@MatchingID int
)
RETURNS  TABLE 
as
return
select
    Stage.StageID

     ,StageType.StageTypeID
     ,StageType.StageTypeCode
     ,StageType.StageTypeName
     ,StageType.StageTypeName_ru

     ,Stage.IterationNumber
     ,Stage.StartDate
     ,Stage.EndPlanDate
     ,Stage.EndDate

     ,Stage.IsCurrent

from
    dbo.Stages Stage with (nolock) 
	
		left join dbo.StagesTypes StageType with (nolock)  on
    StageType.StageTypeID = Stage.StageTypeID

    join Users_Roles ExecutiveUserRole with (nolock) on
    ExecutiveUserRole.MatchingID = Stage.MatchingID

where
    Stage.MatchingID = @MatchingID
  and
    ExecutiveUserRole.UserID = @UserID
  and
    ExecutiveUserRole.RoleID = (	select RoleID
    from
    dbo.Roles with(nolock)
    where
    RoleCode =  3 )--executive

-- Правильный порядок для вывода в интерфейс
--order by 
--	StageType.StageTypeCode desc 
--	,Stage.IterationNumber desc



    GO
/****** Object:  UserDefinedFunction [napp].[get_StudentBasicInfo]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 30.11.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_StudentBasicInfo]
(	
	@StudentID int
)
RETURNS  TABLE 
as
return
SELECT
    Student.[StudentID]
     ,Student.[Info]
     ,Student.[Info2]


FROM
    [dbo].[Students] Student with (nolock)


where
    @StudentID is not null
  and
    Student.StudentID = @StudentID




    GO
/****** Object:  UserDefinedFunction [napp].[get_Technologies_All]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Technologies_All]
(	
)
RETURNS  TABLE 
as
return
SELECT
    Technology.TechnologyCode as TechnologyCode
     ,Technology.TechnologyName_ru as TechnologyName_ru

FROM
    dbo.Technologies Technology with (nolock) 



GO
/****** Object:  UserDefinedFunction [napp].[get_Technologies_WithSelected_ByProject]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Technologies_WithSelected_ByProject]
(	
	@ProjectID int
)
RETURNS  TABLE 
as
return
SELECT
    Technology.TechnologyCode as TechnologyCode
     ,Technology.TechnologyName_ru as TechnologyName_ru
     ,iif(ProjectTechnology.ProjectTechnologyID is not null, 1, 0) as IsSelectedByProject

FROM
    dbo.Technologies Technology with (nolock) 

		left join dbo.Projects_Technologies ProjectTechnology  with (nolock) on
    ProjectTechnology.TechnologyID = Technology.TechnologyID
    and
    ProjectTechnology.ProjectID = @ProjectID


where
    @ProjectID is not null



    GO
/****** Object:  UserDefinedFunction [napp].[get_Technologies_WithSelected_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_Technologies_WithSelected_ByStudent]
(	
	@StudentID int
)
RETURNS  TABLE 
as
return
SELECT
    Technology.TechnologyCode as TechnologyCode
     ,Technology.TechnologyName_ru as TechnologyName_ru
     ,iif(StudentTechnology.StudentTechnologyID is not null, 1, 0) as IsSelectedByStudent

FROM
    dbo.Technologies Technology with (nolock) 

		left join dbo.Students_Technologies StudentTechnology  with (nolock) on
    StudentTechnology.TechnologyID = Technology.TechnologyID
    and
    StudentTechnology.StudentID = @StudentID

where
    @StudentID is not null




    GO
/****** Object:  UserDefinedFunction [napp].[get_UserMatchings_ByRole]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 11.02.2020
-- Update date: 14.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserMatchings_ByRole]
(	
	@UserID int 
	,@RoleCode int
	,@RoleName nvarchar(50)
)
returns table
as 
return
select distinct
    [User].UserID

        ,Match.MatchingID
        ,Match.MatchingName

        ,MatchType.MatchingTypeCode
        ,MatchType.MatchingTypeName
        ,MatchType.MatchingTypeName_ru

from
    dbo.Users [User] with (nolock)

    join dbo.Users_Roles UserRole with (nolock) on
    UserRole.UserID = [User].UserID

    join dbo.Roles [Role] with (nolock) on
    [Role].RoleID = UserRole.RoleID

    left join dbo.Matching Match with (nolock) on
    Match.MatchingID = UserRole.MatchingID

    left join dbo.MatchingType MatchType with (nolock) on
    MatchType.MatchingTypeID = Match.MatchingTypeID

where
    [User].UserID = @UserID
  and
    (
    [Role].RoleCode = coalesce(@RoleCode, -1)
   or
    (
    @RoleCode is null
  and
    [Role].RoleName = @RoleName
    )
    )

;


GO
/****** Object:  UserDefinedFunction [napp].[get_UserRoles_ByMatching]    Script Date: 17.03.2024 12:16:12 ******/



-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 11.02.2020
-- Update date: 04.03.2020
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_UserRoles_ByMatching]
(	
	@UserID int 
	,@MatchingID int
)
returns table
as 
return
select distinct
    UserRole.UserID
--,UserRole.MatchingID
              ,[Role].RoleCode
              ,[Role].RoleName
              ,[Role].RoleName_ru
              ,UserRole.TutorID
              ,UserRole.StudentID

from
    dbo.Users_Roles UserRole with (nolock)

    join dbo.Roles [Role] with (nolock) on
    [Role].RoleID = UserRole.RoleID

where
    UserRole.UserID = @UserID
  and
    UserRole.MatchingID = @MatchingID
;


GO
/****** Object:  UserDefinedFunction [napp].[get_WorkDirections_All]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 
-- Description:	
-- =============================================
create FUNCTION [napp].[get_WorkDirections_All]
()
RETURNS  TABLE 
as
return
SELECT
    WorkDirection.DirectionCode as DirectionCode
     ,WorkDirection.DirectionName_ru as DirectionName_ru

FROM
    dbo.WorkDirections WorkDirection with (nolock) 



GO
/****** Object:  UserDefinedFunction [napp].[get_WorkDirections_WithSelected_ByProject]    Script Date: 17.03.2024 12:16:12 ******/

-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 20.02.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_WorkDirections_WithSelected_ByProject]
(	
	@ProjectID int
)
RETURNS  TABLE 
as
return
SELECT
    WorkDirection.DirectionCode as DirectionCode
     ,WorkDirection.DirectionName_ru as DirectionName_ru
     ,iif(ProjectWorkDirection.ProjectDirectionID is not null, 1, 0) as IsSelectedByProject

FROM
    dbo.WorkDirections WorkDirection with (nolock) 

		left join dbo.Projects_WorkDirections ProjectWorkDirection  with (nolock) on
    ProjectWorkDirection.DirectionID = WorkDirection.DirectionID
    and
    ProjectWorkDirection.ProjectID = @ProjectID

where
    @ProjectID is not null



    GO
/****** Object:  UserDefinedFunction [napp].[get_WorkDirections_WithSelected_ByStudent]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 06.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp].[get_WorkDirections_WithSelected_ByStudent]
(	
	@StudentID int
)
RETURNS  TABLE 
as
return
SELECT
    WorkDirection.DirectionCode as DirectionCode
     ,WorkDirection.DirectionName_ru as DirectionName_ru
     ,iif(StudentWorkDirection.StudentDirectionID is not null, 1, 0) as IsSelectedByStudent

FROM
    dbo.WorkDirections WorkDirection with (nolock) 

		left join dbo.Students_WorkDirections StudentWorkDirection  with (nolock) on
    StudentWorkDirection.DirectionID = WorkDirection.DirectionID
    and
    StudentWorkDirection.StudentID = @StudentID

where
    @StudentID is not null




    GO
/****** Object:  UserDefinedFunction [napp_in].[get_ProjectsPopularity]    Script Date: 17.03.2024 12:16:12 ******/


-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 18.03.2020
-- Update date: 
-- Description:	
-- =============================================
CREATE FUNCTION [napp_in].[get_ProjectsPopularity]
(	
	@MatchingID int
)
RETURNS  TABLE 
as
return
	with Projects 
	as 
	(
		select 
			 Project.ProjectID
			 ,Project.TutorID
			 ,count(Preference.PreferenceID) as PreferenceCount
			 --,count (Preference.PreferenceID) over (partition by Project.TutorID ) 
		from 
			dbo.Projects Project with (nolock)
		
			left join dbo.StudentsPreferences Preference with (nolock) on 
				Preference.ProjectID = Project.ProjectID
	
		where 
			Project.MatchingID = @MatchingID 
			and 
			coalesce(Project.ProjectQuotaQty, -1) != 0
	
		group by 
			Project.ProjectID
			,Project.TutorID
			,Project.ProjectQuotaQty
	)
select --top 5
    Project.ProjectID
     ,Project.TutorID
     ,Project.PreferenceCount as PreferenceCount_ByProject
     ,sum(Project.PreferenceCount) over (partition by Project.TutorID) as PreferenceCount_ByTutor
from
    Projects as Project
    --order by 
    --	sum(Project.PreferenceCount) over (partition by TutorID) 
    --	,Project.PreferenceCount



    GO
/****** Object:  View [dbo_v].[AvailableStudentsPreferences]    Script Date: 17.03.2024 12:16:12 ******/
CREATE view [dbo_v].[AvailableStudentsPreferences]
as
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
    ,Student.MatchingID
    from
    dbo.StudentsPreferences Preference with (nolock)

    join dbo.Students Student with(nolock) on
    Student.StudentID = Preference.StudentID
    where
    Preference.IsAvailable = 1
    and
    Preference.IsUsed = 0
)
select
    Preference.PreferenceID
     ,Preference.StudentID
     ,Preference.ProjectID
     ,Preference.OrderNumber
     ,Preference.IsInUse
     --,iif(Preference.IsInUse = 1, 1, null) IsFromPreviousIteration --Если Preference была в IsInUse, значит он приехала из прошлой итерации (иначе - это новый студик в списке у препода) 
     ,Preference.MatchingID

from
    AvailableStudentsPreferences Preference

where
    Preference.OrderNumber = Preference.MinOrderNumber

    GO
/****** Object:  View [dbo_v].[Users_FullInfo]    Script Date: 17.03.2024 12:16:12 ******/







-- =============================================
-- Author:		Antyukhova Ekaterina
-- Create date: 12.02.2020
-- Update date: 07.03.2020
-- Description:	
-- =============================================
CREATE view [dbo_v].[Users_FullInfo]
as
select
    [User].UserID
        ,[User].Surname
        ,[User].Name
        ,[User].Patronimic
        ,UserRole.LastVisitDate
        ,UserRole.MatchingID

        ,[User].Surname
    + ' ' + iif ([User].Name is not null, substring([User].Name,1, 1) , '' ) + '.'
    + ' ' + iif ([User].Patronimic is not null, substring([User].Patronimic,1, 1) , '' ) + '.'
    as NameAbbreviation

    --,coalesce(UserRole.StudentID, UserRole.TutorID) as EntityID
        ,UserRole.StudentID
        ,UserRole.TutorID

        ,Role.RoleID
        ,Role.RoleCode
        ,Role.RoleType
        ,Role.RoleName
        ,Role.RoleName_ru

        ,[Group].GroupID
        ,[Group].GroupName


from
    dbo.Users [User] with (nolock)

    left join dbo.Users_Roles UserRole with (nolock) on
    UserRole.UserID = [User].UserID

    left join dbo.Roles Role with (nolock) on
    Role.RoleID = UserRole.RoleID

    left join dbo.Students Student on
    Student.StudentID = UserRole.StudentID

    left join dbo.Groups [Group] with (nolock) on
    [Group].GroupID = Student.GroupID








    GO
