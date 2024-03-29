USE [Market]
GO
/****** Object:  StoredProcedure [dbo].[spPriceBulkInsert]    Script Date: 27/07/2020 12:32:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPriceBulkInsert]
	@CreatedBy varchar(50),
	@UpdatedBy varchar(50),
	@PricesJson varchar(max),
	@InsertedID int OUTPUT -- 0 = no record insertion
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRY

		INSERT INTO dbo.Price
		(
			pric_Date,
			pric_MarketPrice,
			pric_CreatedTimeStamp,
			pric_CreatedBy,
			pric_UpdatedTimeStamp,
			pric_UpdatedBy
		)
		SELECT
		PriceDate,
		--CONVERT(varchar(16), CONVERT(datetime, PriceDate, 103), 120), -- convert date from DD/MM/YYYY to YYYY-MM-DD
		MarketPrice,
		GETDATE(),
		@CreatedBy,
		GETDATE(),
		@UpdatedBy
		FROM OPENJSON(@PricesJson)
		WITH
		(
			/*PriceDate varchar(16),*/
			PriceDate datetime,
			MarketPrice float
		);

		SET @InsertedID = SCOPE_IDENTITY();

	END TRY
	BEGIN CATCH
		DECLARE @argApplicationName nvarchar(MAX) = 'SQL Server: ' + DB_NAME() + ' ' + OBJECT_NAME(@@PROCID);
		DECLARE @argErrorMessage nvarchar(MAX) = ERROR_MESSAGE();
		DECLARE @argUserName nvarchar(MAX) = SUSER_SNAME();

		EXECUTE dbo.spLogMessage
		@ApplicationName = @argApplicationName,
		@ErrorMessage = @argErrorMessage,
		@CreatedBy = @argUserName;

		THROW;
	END CATCH

END
