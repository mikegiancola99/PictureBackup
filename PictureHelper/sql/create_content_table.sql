USE [pictures]
GO

/****** Object:  Table [dbo].[content]    Script Date: 03/12/2014 23:46:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[content](
	[md5] [nchar](40) NOT NULL,
	[filename] [nchar](40) NOT NULL,
	[directory] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO


