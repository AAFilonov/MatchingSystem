INSERT INTO [ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (1, 1, 'Self', 'Пользовательский');
INSERT INTO [ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (2, 2, 'Auto', 'Автоматический');
INSERT INTO [ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (3, 3, 'Direct', 'Назначен ответсвенным (жестко)');
INSERT INTO [ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (4, 4, 'End', 'Назначен ответсвенным (в конце)');

INSERT INTO [MatchingType] ([MatchingTypeID], [MatchingTypeName], [MatchingTypeName_ru], [MatchingTypeCode]) VALUES (1, 'baccalaureate', 'бакалавриат', '1');
INSERT INTO [MatchingType] ([MatchingTypeID], [MatchingTypeName], [MatchingTypeName_ru], [MatchingTypeCode]) VALUES (2, 'magistracy', 'магистратура', '2');

INSERT INTO [QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (1, 1, 'active', 'действительна');
INSERT INTO [QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (3, 2, 'requested', 'отправлен запрос');
INSERT INTO [QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (4, 3, 'declined', 'отклонена');
INSERT INTO [QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (5, 4, 'unactive', 'не действительна');

INSERT INTO [Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (1, 1, 'Tutor', 1, 'Преподаватель');
INSERT INTO [Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (2, 2, 'Student', 1, 'Студент');
INSERT INTO [Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (3, 3, 'Executive', 2, 'Ответственный');
INSERT INTO [Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (4, 4, 'GroupMonitor', 2, 'Староста');
INSERT INTO [Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (5, 5, 'Admin', 2, 'Администратор');

INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (1, 1, 'Matching initialization', 'Создание распределения');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (2, 2, 'Matching preparation', 'Подготовка распределения');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (3, 3, 'Accumulation student''s preferences', 'Сбор предпочтений студентов');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (4, 4, 'Iteration', 'Итерация');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (6, 5, 'Manual adjustment', 'Ручная корректировка распределения');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (7, 6, 'Final', 'Финал');
INSERT INTO [StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (8, 7, 'Archive', 'Архив');

INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (1, 'JS', 1);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (2, 'NodeJS', 2);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (4, 'PHP', 3);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (5, 'C/C++', 4);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (6, 'C#', 5);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (7, 'SQL', 6);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (8, 'Web-приложение', 7);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (9, 'Desktop-приложение', 8);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (10, 'Мобильное приложение (Android)', 9);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (11, 'Мобильное приложение (IOS)', 10);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (13, 'БД/ХД', 11);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (15, 'Python', 12);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (16, 'ASP', 13);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (18, 'Мобильное приложение (Windows Phone)', 14);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (19, '1C', 15);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (20, 'JS/ AJAX/ JQuery', 16);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (21, 'Web-фреймворки', 17);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (22, 'Delphi', 18);
INSERT INTO [Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (23, 'Multidimensional/ Tabular Models', 19);

INSERT INTO [WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (1, 'Все равно', 1);
INSERT INTO [WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (2, 'Научно-исследовательский проект', 2);
INSERT INTO [WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (3, 'Прикладное ПО', 3);
INSERT INTO [WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (4, 'Прикладное ПО (выполняется по заявке, предполагается внедрение)', 4);
INSERT INTO [WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (5, 'Типовой учебный проект', 5);
