USE [Market]
GO
/****** Object:  StoredProcedure [dbo].[spLogMessage]    Script Date: 27/07/2020 12:31:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spLogMessage]
	@ApplicationName nvarchar(50),
	@ErrorMessage nvarchar(250),
	@CreatedBy varchar(50)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.ErrorLog
	(
		erro_ApplicationName,
		erro_Message,
		erro_CreatedTimeStamp,
		erro_CreatedBy,
		erro_UpdatedTimeStamp,
		erro_UpdatedBy
	)
	VALUES
	(
		@ApplicationName,
		@ErrorMessage,
		GETDATE(),
		@CreatedBy,
		GETDATE(),
		@CreatedBy
	);

END
