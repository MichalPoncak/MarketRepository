USE [Market]
GO

/****** Object:  Table [dbo].[ErrorLog]    Script Date: 27/07/2020 12:33:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ErrorLog](
	[erro_ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[erro_ApplicationName] [nvarchar](50) NOT NULL,
	[erro_Message] [nvarchar](250) NOT NULL,
	[erro_CreatedTimeStamp] [datetime] NOT NULL,
	[erro_CreatedBy] [varchar](50) NOT NULL,
	[erro_UpdatedTimeStamp] [datetime] NOT NULL,
	[erro_UpdatedBy] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[erro_ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


