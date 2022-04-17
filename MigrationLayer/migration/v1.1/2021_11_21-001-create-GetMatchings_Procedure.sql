SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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