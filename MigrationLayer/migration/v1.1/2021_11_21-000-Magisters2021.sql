SET NOCOUNT on;
USE [DiplomaMatching]
go

Declare @NEW_MATCHING_NAME nvarchar(max) = 'Распределение магистрантов 1 курса 2021';
Declare @TYPE_MATCHING int = 2; --1 - Бакалавры, 2 - Магистры.
Declare @GROUP_NAME1 nvarchar(max) = 'О-21-ИВТ-ипо-М';
Declare @GROUP_NAME2 nvarchar(max) = 'О-21-ПРИ-ппс-М';


DECLARE @InputUsers nvarchar(max) = 'Артамошин Алексей Владимирович/О-21-ИВТ-ипо-М;Беляков Никита Владимирович/О-21-ИВТ-ипо-М;Горелова Татьяна/О-21-ИВТ-ипо-М;Дорощук Кирилл Андреевич/О-21-ИВТ-ипо-М;Зуйков Владимир Николаевич/О-21-ИВТ-ипо-М;Калинин Владислав Сергеевич/О-21-ИВТ-ипо-М;Масумов Фарзонхон Фуркатович/О-21-ИВТ-ипо-М;Нагибин Виктор Алексеевич/О-21-ИВТ-ипо-М;Назаров Алексей Александрович/О-21-ИВТ-ипо-М;Перепечко Валерий Владимирович/О-21-ИВТ-ипо-М;Степин Александр Александрович/О-21-ИВТ-ипо-М;Чайко Анна Александровна/О-21-ИВТ-ипо-М;Шевцов Дмитрий Владимирович/О-21-ИВТ-ипо-М;Шкурко Антон Борисович/О-21-ИВТ-ипо-М;Власкин Эрик Русланович/О-21-ПРИ-ппс-М;Капралова Ксения Максимовна/О-21-ПРИ-ппс-М;Копылов Андрей Андреевич/О-21-ПРИ-ппс-М;Панаскин Владимир Вячеславович/О-21-ПРИ-ппс-М;Питикин Анатолий Андреевич/О-21-ПРИ-ппс-М;Протасов Дмитрий Александрович/О-21-ПРИ-ппс-М;Селифонтов Андрей Александрович/О-21-ПРИ-ппс-М;Сополаев Никита Алексеевич/О-21-ПРИ-ппс-М;Субботин Владислав Владимирович/О-21-ПРИ-ппс-М;Сухарев Евгений Александрович/О-21-ПРИ-ппс-М;Трунников Максим Владиславович/О-21-ПРИ-ппс-М;Фомин Илья Игоревич/О-21-ПРИ-ппс-М;';


--Создание нового Matchig
insert into Matching(MatchingName,MatchingTypeID,CreatorUserID) Values(@NEW_MATCHING_NAME,@TYPE_MATCHING,2)
Declare @CurrentMatchID int = @@IDENTITY;
Print 'New Matching {id=' + Cast(@CurrentMatchID as nvarchar(5)) +'}';

--Установка флага активного Stage = 0
--update Stages SET IsCurrent=0; wh
--PRINT 'Предыдущие Stage установлены в состояние НЕАКТИВНО'


--Добавление новой Stage
insert into Stages (StageTypeID,StartDate,EndPlanDate,IsCurrent,MatchingID) VALUES(1,CURRENT_TIMESTAMP,DATEADD(HOUR,1, CURRENT_TIMESTAMP),1,@CurrentMatchID)
Declare @StagID int = @@IDENTITY;
Print 'New Stage = ' + cast(@StagID as nvarchar(5));

--Добавление ответственных
with UsersID as(
    select 29  as UserID
    union
    select 102 as UserID
    union
    select 103 as UserID
)
insert into Users_Roles (UserID,RoleID,MatchingID)
select UserID,3,@CurrentMatchID from UsersID

Declare @OTVETSTs nvarchar(max);
select @OTVETSTs =STRING_AGG(coalesce(Surname,login),',') from Users where UserID in (29,102,103)
    PRINT 'Ответсвенные ' + @OTVETSTs


--Добавление Групп
insert into Groups(GroupName,MatchingID) VALUES(@GROUP_NAME1,@CurrentMatchID);
Declare @GROUP_ID1 int= @@IDENTITY;
PRINT 'New Group ' + @GROUP_NAME1 + ' {id='+cast(@GROUP_ID1 as nvarchar(11)) + '}'

insert into Groups(GroupName,MatchingID) VALUES(@GROUP_NAME2,@CurrentMatchID);
Declare @GROUP_ID2 int= @@IDENTITY;
PRINT 'New Group ' + @GROUP_NAME2 + ' {id='+cast(@GROUP_ID2 as nvarchar(11)) + '}'


--Преподаватели
drop table if exists #Tutors ;
create table  #Tutors (
                          UserID int NULL,
                          TutorID int NULL,
                          FullName nvarchar(max) NULL
)
;

with tut as(
    select replace(Surname,'ё','е') + ' ' + LEFT(Name,1) + '.'+ LEFT(Patronimic,1)+'.' as FUllName,* from Users
    )
        , UsersID as(
select tut.UserID,FUllName  from tut
where
    FULLName like 'Азарченков%'
   or FULLName like 'Белов%'
   or FULLName like 'Булатицкий%'
   or FULLName like 'Горбунов%'
   or FULLName like 'Гулаков К.В%'
   or FULLName like 'Дергачев%'
   or FULLName like 'Дмитроченко%'
   or FULLName like 'Израилев%'
   or FULLName like 'Копелиович%'
   or FULLName like 'Коростелев%'
   or FULLName like 'Лагерев%'
   or FULLName like 'Подвесовская%'
   or FULLName like 'Подвесовский%'
   or FULLName like 'Титарев%'
   or FULLName like 'Трубаков А.О.%'
   or FULLName like 'Трубаков Е.О.%'
   or FULLName like 'Шалимов%'
    )
insert into #Tutors (FullName,UserID)
select distinct FUllName,UserID from UsersID
;
Declare @countTutors int = (select count(Distinct UserID) from #Tutors)
PRINT 'Count Tuturs ' + cast(@countTutors as nvarchar(11))


DECLARE @CursorUserID int;
DECLARE @CursorTutorID int;
DECLARE @CursorFULLName nvarchar(max);

DECLARE @cursorTotor CURSOR;


SET @cursorTotor = CURSOR SCROLL FOR
select FullName,TutorID,UserID from #Tutors;
OPEN @cursorTotor
    fetch next from @cursorTotor into @CursorFullName,@CursorTutorID,@CursorUserID	

While @@FETCH_STATUS = 0
begin
insert into Tutors (MatchingID) Values (@CurrentMatchID)
Declare @TutorID int = @@IDENTITY;
Insert into Users_Roles (MatchingID,RoleID,UserID,TutorID) Values(@CurrentMatchID,1,@CursorUserID,@TutorID)
Update #Tutors SET TutorID = @TutorID where FullName = @CursorFullName
    fetch next from @cursorTotor into @CursorFullName,@CursorTutorID,@CursorUserID
end
CLOSE @cursorTotor;
DEALLOCATE @cursorTotor;
--select * from #Tutors;

--Студенты
drop table if exists #Students ;
create table  #Students (
                            UserID int NULL,
                            StudentsID int NULL,
                            FullName nvarchar(max) NULL,
                            GroupID int NULL
)
;

--очистить последний разделитель
--select @InputUsers = iif(right(@InputUsers,1)=';',LEFT(@InputUsers,LEN(@InputUsers)-1),@InputUsers);

--Распарсить и записать 
with InputStudentGroup as(
    select left(value,charindex('/',value)-1) as FULLName,SUBSTRING(value,charindex('/',value)+1,len(@InputUsers)) as GroupName
from string_split(@InputUsers,';')
where value <>''
    )
insert #Students (FullName,GroupID)
select
    FULLName,IIF(GroupName = @GROUP_NAME1,@Group_ID1,@Group_ID2) as GroupID
from InputStudentGroup

;
merge #Students as dst
    using (
    select
    *,
    replace(Surname,'ё','е') + ' ' + replace(Name,'ё','е')  + ' '+ replace(Patronimic,'ё','е') as FUllName
    from Users
    ) as src
    on (
    dst.FUllName = src.FUllName
    COLLATE Cyrillic_General_CI_AS
    )
    when Matched
    then
Update
    SET UserID = src.UserID
;
--select * from #Students;

DECLARE @CursorStudentID int;
DECLARE @CursorGroupID int;
DECLARE @cursorStudent CURSOR;


SET @cursorStudent = CURSOR SCROLL FOR
select FullName,StudentsID,UserID,GroupID from #Students
                                               --where UserID is null
    OPEN @cursorStudent
fetch next from @cursorStudent into @CursorFullName,@CursorStudentID,@CursorUserID,@CursorGroupID

    While @@FETCH_STATUS = 0
begin
	declare @cFULLName nvarchar(max) = @CursorFULLName;
	IF @CursorUserID is null
BEGIN
		--PRINT @CursorFULLName;	
		DECLARE @Surname nvarchar(max) = (select left(@CursorFULLName,charindex(' ',@CursorFULLName)-1));
		--select @Surname;
Select @CursorFULLName = SUBSTRING(@CursorFULLName,charindex(' ',@CursorFULLName)+1,LEN(@CursorFULLName));

DECLARE @Name nvarchar(max);
select @Name = iif(len(@CursorFULLName)<>0,
                   iif(charindex(' ',@CursorFULLName)<>0,
                       left(@CursorFULLName,charindex(' ',@CursorFULLName)-1),
                       @CursorFULLName
                       )
    ,@CursorFULLName);


--iif(charindex(' ',@CursorFULLName)<>0,
--,null)
DECLARE @Patronimic nvarchar(max);
Select  @Patronimic = iif(charindex(' ',@CursorFULLName)>0,
                          SUBSTRING(@CursorFULLName,charindex(' ',@CursorFULLName),LEN(@CursorFULLName)),
                          NULL
    );

DECLARE @login as nvarchar(max) = @Surname+coalesce('_'+@Name,'');

		DECLARE	@Result	VarChar(max)
		SET	@Result	= @login
SELECT	@Result	= Replace(@Result,Rus,Lat)
FROM	(	SELECT 'А','A'
             UNION ALL	SELECT 'Б','B'
             UNION ALL	SELECT 'В','V'
             UNION ALL	SELECT 'Г','G'
             UNION ALL	SELECT 'Д','D'
             UNION ALL	SELECT 'Е','E'
             UNION ALL	SELECT 'Ё','YO'
             UNION ALL	SELECT 'Ж','ZH'
             UNION ALL	SELECT 'З','Z'
             UNION ALL	SELECT 'И','I'
             UNION ALL	SELECT 'Й','Y'
             UNION ALL	SELECT 'К','K'
             UNION ALL	SELECT 'Л','L'
             UNION ALL	SELECT 'М','M'
             UNION ALL	SELECT 'Н','N'
             UNION ALL	SELECT 'О','O'
             UNION ALL	SELECT 'П','P'
             UNION ALL	SELECT 'Р','R'
             UNION ALL	SELECT 'С','S'
             UNION ALL	SELECT 'Т','T'
             UNION ALL	SELECT 'У','U'
             UNION ALL	SELECT 'Ф','F'
             UNION ALL	SELECT 'Х','H'
             UNION ALL	SELECT 'Ц','C'
             UNION ALL	SELECT 'Ч','CH'
             UNION ALL	SELECT 'Ш','SH'
             UNION ALL	SELECT 'Щ','SH'
             UNION ALL	SELECT 'Ъ',''
             UNION ALL	SELECT 'Ы','Y'
             UNION ALL	SELECT 'Ь',''
             UNION ALL	SELECT 'Э','E'
             UNION ALL	SELECT 'Ю','YU'
             UNION ALL	SELECT 'Я','YA')T(Rus,Lat)
WHERE	@login LIKE '%'+Rus+'%'

select @Result = iif((select count(*) as cnt from Users where Login =@Result)>0,@Result + CAST(YEAR(CURRENT_TIMESTAMP)as nvarchar(max)),@Result);

PRINT @Result;

insert into Users(Surname,Name,Patronimic,Login,PasswordHash) VALUES(@Surname,@Name,@Patronimic,@Result,'$s2$16384$8$1$x/2cqJccoE4iBpnPA4sT6PLXMux50R2I1WpJXOw16m4=$8YEsl1cS1OvkO5tmDaS8+Uo4zw7109y71Y9bSU+rSGE=')
    SET @CursorUserID = @@IDENTITY;


Update #Students set UserID = @CursorUserID where #Students.FullName = @cFULLName and #Students.GroupID = @CursorGroupID
END

insert into Students(GroupID,MatchingID) VALUES(@CursorGroupID,@CurrentMatchID)
Declare @StudentID int = @@IDENTITY;
UPDATE #Students SET StudentsID = @StudentID where #Students.FullName = @cFULLName and #Students.GroupID = @CursorGroupID

    insert INTO Users_Roles (RoleID,MatchingID,UserID,StudentID)VALUES(2,@CurrentMatchID,@CursorUserID,@StudentID);

fetch next from @cursorStudent into @CursorFullName,@CursorStudentID,@CursorUserID,@CursorGroupID
end
CLOSE @cursorStudent;
DEALLOCATE @cursorStudent;

--select * from #Students


--Добавление в Students


;
merge
    Tutors_Groups as trg
    using (
    select * from #Tutors
    ) as src
    on (src.TutorID = trg.TutorID and trg.GroupID = @GROUP_ID1)
    when not matched
    THEN
    insert (TutorID,GroupID) VALUES(src.TutorID,@GROUP_ID1)

;


merge
    Tutors_Groups as trg
    using (
    select * from #Tutors
    ) as src
    on (src.TutorID = trg.TutorID and trg.GroupID = @GROUP_ID2)
    when not matched
    THEN
    insert (TutorID,GroupID) VALUES(src.TutorID,@GROUP_ID2)
;



merge
    CommonQuotas as trg
    using (
    select * from #Tutors
    )as src
    on (trg.TutorID = src.TutorID)
    when not Matched
    then insert (TutorID,QTY,CreateDate,QuotaStateID,isNotification,StageID) VALUES(src.TutorID,2,CURRENT_TIMESTAMP,1,0,@StagID)
;


---УДАЛИТЬ ПЕРЕД ОТПРАВКОЙ
insert into Tutors (MatchingID)VALUES(5)
DECLARE @ATutorID int = @@IDENTITY;

INSERT into Users_Roles (MatchingID,RoleID,UserID,TutorID) VALUES (@CurrentMatchID,1,102,@ATutorID);
PRINT 'AAA Tutor - UserID =102 TutorID='+ cast(@ATutorID as nvarchar(11))
INSERT INTO CommonQuotas(TutorID,QTY,CreateDate,QuotaStateID,isNotification,StageID) vALUES(@ATutorID,2,CURRENT_TIMESTAMP,1,0,@StagID)

	--СОздание проекта ИВТ каид для ивт
	insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ИВТ-каид',@ATutorID)
	Declare @Project int = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID1);

--СОздание проекта ИВТ ипо для ивт
insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ИВТ-ипо',@ATutorID)
    SET @Project = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID1);

--СОздание проекта ПРИ для при
insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ПРИ',@ATutorID)
    SET @Project = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID2);

--select * from Projects

--select * from Students where StudentID = 24
--select * from Users_Roles where StudentID = 24
--select * from Users where UserID = 53

--select* from napp.get_Projects_ByTutor(50)
--select* from napp.get_Projects_ByTutor(79)
--select top 1 * from napp.get_Projects_ByStudent(150)
--select top 1 * from napp.get_Projects_ByStudent(204)
--select * from Students
---УДАЛИТЬ ПЕРЕД ОТПРАВКОЙ


update Stages SET IsCurrent =0, EndDate=CURRENT_TIMESTAMP where StageID = @StagID
    insert into Stages (StageTypeID,StartDate,EndPlanDate,IsCurrent,MatchingID) VALUES(2,CURRENT_TIMESTAMP,DATEADD(HOUR,1, CURRENT_TIMESTAMP),1,@CurrentMatchID)
SET @StagID = @@IDENTITY;

--Создание проектов
--select * from Projects

DECLARE @cursorTutor CURSOR;
SET @cursorTutor = CURSOR SCROLL FOR
select FullName,TutorID,UserID from #Tutors;
OPEN @cursorTutor
    fetch next from @cursorTutor into @CursorFullName,@CursorTutorID,@CursorUserID	

While @@FETCH_STATUS = 0
begin
	--СОздание проекта ИВТ каид для ивт
insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ИВТ-каид',@CursorTutorID)
    SET @Project = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID1);

--СОздание проекта ИВТ ипо для ивт
insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ИВТ-ипо',@CursorTutorID)
    SET @Project = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID1);

--СОздание проекта ПРИ для при
insert into Projects(MatchingID,CreateDate,IsDefault,IsClosed,ProjectQuotaQty,ProjectName,TutorID) VALUES(@CurrentMatchID,CURRENT_TIMESTAMP,0,0,1,'ПРИ',@CursorTutorID)
    SET @Project = @@IDENTITY;
insert into Projects_Groups(ProjectID,GroupID) VALUES(@Project,@GROUP_ID2);

fetch next from @cursorTutor into @CursorFullName,@CursorTutorID,@CursorUserID
end
CLOSE @cursorTutor;
DEALLOCATE @cursorTutor;


--INSERT INTO Projects(ProjectName,MatchingID,IsDefault,CreateDate)VALUES('ИВТ-каид',@CurrentMatchID,0,CURRENT_TIMESTAMP) ('ИВТ-ипо',@CurrentMatchID),('ПРИ',@CurrentMatchID);

--Создание проектов


--exec napp.goto_NextStage @CurrentMatchID ;


--select * from napp.get_CurrentStage_ByMatching(5)



--Удаление временных таблиц
drop table if exists #Tutors ;
drop table if exists #Students ;
delete  from dbo.Users_Roles  where TutorID = '79';