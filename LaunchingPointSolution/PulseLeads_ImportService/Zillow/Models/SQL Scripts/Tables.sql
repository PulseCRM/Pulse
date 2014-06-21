
DROP TABLE [dbo].[ZillowLogger]
GO

DROP TABLE [dbo].[ZillowImports]
GO

DROP TABLE [dbo].[ZillowAttributes]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZillowAttributes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImportTransId] [uniqueidentifier] NULL,
	[Version] [nvarchar](10) NULL,
	[GroupName] [nvarchar](255) NULL,
	[AttributeName] [nvarchar](255) NULL,
	[AttributeValue] [nvarchar](255) NULL,
	[ImportedFlag] [bit] NULL,
	[BorrowerId] [int] NULL,
	[CoborrowerId] [int] NULL,
	[LoanId] [int] NULL,
 CONSTRAINT [PK_ZillowAttributes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZillowImports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImportDate] [datetime] NULL,
	[ReturnStatus] [nvarchar](50) NULL,
	[ZillowXml] [ntext] NULL,
	[ImportTransId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ZillowImports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZillowLogger](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[thread] [nvarchar](255) NOT NULL,
	[level] [nvarchar](50) NOT NULL,
	[logger] [nvarchar](255) NOT NULL,
	[message] [nvarchar](4000) NOT NULL,
	[exception] [nvarchar](4000) NULL,
 CONSTRAINT [PK_ZillowLogger] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


