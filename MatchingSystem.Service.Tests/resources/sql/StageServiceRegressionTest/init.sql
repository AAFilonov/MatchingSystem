SET IDENTITY_INSERT [dbo].[Matching] ON
INSERT [dbo].[Matching] ([MatchingID], [MatchingName], [MatchingTypeID], [CreatorUserID]) VALUES (1, N'Тест', 1, 1)
SET IDENTITY_INSERT [dbo].[Matching] OFF

SET IDENTITY_INSERT [dbo].[Stages] ON
INSERT [dbo].[Stages] ([StageID], [StageTypeID], [StageName], [IterationNumber], [IsCurrent], [MatchingID]) VALUES (1, 3, NULL, NULL, 1, 1)
SET IDENTITY_INSERT [dbo].[Stages] OFF

SET IDENTITY_INSERT [dbo].[Tutors] ON
INSERT [dbo].[Tutors] ([TutorID], [TutorBK], [IsClosed], [CloseIterationNumber], [IsReadyToStart], [MatchingID]) VALUES (1, 1, 0, NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[Tutors] OFF

SET IDENTITY_INSERT [dbo].[CommonQuotas] ON
INSERT [dbo].[CommonQuotas] ([CommonQuotaID], [TutorID], [Qty], [CreateDate], [QuotaStateID], [UpdateDate], [IsNotification], [Message], [StageID]) VALUES (1, 1, 3, CAST(N'2020-05-16T00:00:00.000' AS DateTime), 1, NULL, 0, NULL, 1)
SET IDENTITY_INSERT [dbo].[CommonQuotas] OFF

SET IDENTITY_INSERT [dbo].[Projects] ON
INSERT [dbo].[Projects] ([ProjectID], [ProjectName], [Info], [TutorID], [IsClosed], [IsDefault], [MatchingID], [ProjectQuotaQty], [CloseStage], [ProjectQuotaDelta]) VALUES (1, N'Проект 1', NULL, 1, 0, 1, 1, 1, NULL, NULL)
INSERT [dbo].[Projects] ([ProjectID], [ProjectName], [Info], [TutorID], [IsClosed], [IsDefault], [MatchingID], [ProjectQuotaQty], [CloseStage], [ProjectQuotaDelta]) VALUES (2, N'Проект 2', NULL, 1, 0, 1, 1, 1, NULL, NULL)
INSERT [dbo].[Projects] ([ProjectID], [ProjectName], [Info], [TutorID], [IsClosed], [IsDefault], [MatchingID], [ProjectQuotaQty], [CloseStage], [ProjectQuotaDelta]) VALUES (3, N'Проект 3', NULL, 1, 0, 1, 1, 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Projects] OFF

SET IDENTITY_INSERT [dbo].[Groups] ON
INSERT [dbo].[Groups] ([GroupID], [GroupBK], [GroupName], [MatchingID]) VALUES (1, NULL, N'Группа 1', 1)
SET IDENTITY_INSERT [dbo].[Groups] OFF

SET IDENTITY_INSERT [dbo].[Students] ON
INSERT [dbo].[Students] ([StudentID], [StudentBK], [GroupID], [Info], [MatchingID], [Info2])  VALUES (1, 1, 1, N's1', 1, NULL )
INSERT [dbo].[Students] ([StudentID], [StudentBK], [GroupID], [Info], [MatchingID], [Info2])  VALUES (2, 2, 1, N's2', 1, NULL )
INSERT [dbo].[Students] ([StudentID], [StudentBK], [GroupID], [Info], [MatchingID], [Info2])  VALUES (3, 3, 1, N's3', 1, NULL )
SET IDENTITY_INSERT [dbo].[Students] OFF





