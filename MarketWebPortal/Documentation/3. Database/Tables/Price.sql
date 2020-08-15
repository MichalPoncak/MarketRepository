USE [Market]
GO

/****** Object:  Table [dbo].[Price]    Script Date: 27/07/2020 12:33:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Price](
	[pric_PriceID] [int] IDENTITY(1,1) NOT NULL,
	[pric_Date] [datetime] NOT NULL,
	[pric_MarketPrice] [decimal](19, 8) NOT NULL,
	[pric_CreatedTimeStamp] [datetime] NOT NULL,
	[pric_CreatedBy] [varchar](50) NOT NULL,
	[pric_UpdatedTimeStamp] [datetime] NOT NULL,
	[pric_UpdatedBy] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED 
(
	[pric_PriceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


