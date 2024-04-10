SET IDENTITY_INSERT [dbo].[ChoosingTypes] ON
GO
INSERT [dbo].[ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (1, 1, N'Self', N'Пользовательский')
GO
INSERT [dbo].[ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (2, 2, N'Auto', N'Автоматический')
GO
INSERT [dbo].[ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (3, 3, N'Direct', N'Назначен ответсвенным (жестко)')
GO
INSERT [dbo].[ChoosingTypes] ([TypeID], [TypeCode], [TypeName], [TypeName_ru]) VALUES (4, 4, N'End', N'Назначен ответсвенным (в конце)')
GO
SET IDENTITY_INSERT [dbo].[ChoosingTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[MatchingType] ON
GO
INSERT [dbo].[MatchingType] ([MatchingTypeID], [MatchingTypeName], [MatchingTypeName_ru], [MatchingTypeCode]) VALUES (1, N'baccalaureate', N'бакалавриат', N'1')
GO
INSERT [dbo].[MatchingType] ([MatchingTypeID], [MatchingTypeName], [MatchingTypeName_ru], [MatchingTypeCode]) VALUES (2, N'magistracy', N'магистратура', N'2')
GO
SET IDENTITY_INSERT [dbo].[MatchingType] OFF
GO
SET IDENTITY_INSERT [dbo].[QuotasStates] ON
GO
INSERT [dbo].[QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (1, 1, N'active', N'действительна')
GO
INSERT [dbo].[QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (3, 2, N'requested', N'отправлен запрос')
GO
INSERT [dbo].[QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (4, 3, N'declined', N'отклонена')
GO
INSERT [dbo].[QuotasStates] ([QuotaStateID], [QuotaStateCode], [QuotaStateName], [QuotaStateName_ru]) VALUES (5, 4, N'unactive', N'не действительна')
GO
SET IDENTITY_INSERT [dbo].[QuotasStates] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
GO
INSERT [dbo].[Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (1, 1, N'Tutor', 1, N'Преподаватель')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (2, 2, N'Student', 1, N'Студент')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (3, 3, N'Executive', 2, N'Ответственный')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (4, 4, N'GroupMonitor', 2, N'Староста')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleCode], [RoleName], [RoleType], [RoleName_ru]) VALUES (5, 5, N'Admin', 2, N'Администратор')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[StagesTypes] ON
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (1, 1, N'Matching initialization', N'Создание распределения')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (2, 2, N'Matching preparation', N'Подготовка распределения')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (3, 3, N'Accumulation student''s preferences', N'Сбор предпочтений студентов')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (4, 4, N'Iteration', N'Итерация')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (6, 5, N'Manual adjustment', N'Ручная корректировка распределения')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (7, 6, N'Final', N'Финал')
GO
INSERT [dbo].[StagesTypes] ([StageTypeID], [StageTypeCode], [StageTypeName], [StageTypeName_ru]) VALUES (8, 7, N'Archive', N'Архив')
GO
SET IDENTITY_INSERT [dbo].[StagesTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Technologies] ON
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (1, N'JS', 1)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (2, N'NodeJS', 2)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (4, N'PHP', 3)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (5, N'C/C++', 4)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (6, N'C#', 5)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (7, N'SQL', 6)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (8, N'Web-приложение', 7)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (9, N'Desktop-приложение', 8)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (10, N'Мобильное приложение (Android)', 9)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (11, N'Мобильное приложение (IOS)', 10)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (13, N'БД/ХД', 11)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (15, N'Python', 12)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (16, N'ASP', 13)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (18, N'Мобильное приложение (Windows Phone)', 14)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (19, N'1C', 15)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (20, N'JS/ AJAX/ JQuery', 16)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (21, N'Web-фреймворки', 17)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (22, N'Delphi', 18)
GO
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName_ru], [TechnologyCode]) VALUES (23, N'Multidimensional/ Tabular Models', 19)
GO
SET IDENTITY_INSERT [dbo].[Technologies] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkDirections] ON
GO
INSERT [dbo].[WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (1, N'Все равно', 1)
GO
INSERT [dbo].[WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (2, N'Научно-исследовательский проект', 2)
GO
INSERT [dbo].[WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (3, N'Прикладное ПО', 3)
GO
INSERT [dbo].[WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (4, N'Прикладное ПО (выполняется по заявке, предполагается внедрение)', 4)
GO
INSERT [dbo].[WorkDirections] ([DirectionID], [DirectionName_ru], [DirectionCode]) VALUES (5, N'Типовой учебный проект', 5)
GO
SET IDENTITY_INSERT [dbo].[WorkDirections] OFF
GO
