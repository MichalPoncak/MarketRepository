USE [Market]
GO
/****** Object:  StoredProcedure [dbo].[spPriceGet]    Script Date: 27/07/2020 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPriceGet]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM dbo.Price
END
