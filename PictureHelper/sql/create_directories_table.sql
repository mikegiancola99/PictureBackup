USE [pictures]
GO

/****** Object:  Table [dbo].[directories]    Script Date: 03/12/2014 23:47:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[directories](
	[path] [nchar](255) NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO


