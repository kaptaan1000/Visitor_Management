SELECT * FROM VISITOR

CREATE TABLE [dbo].[VISITOR](
	[COMP] [char](4) NULL,
	[UNIT] [char](6) NULL,
	[DATE] [datetime] NULL,
	[VSNM] [varchar](250) NULL,
	[VSCNM] [varchar](250) NULL,
	[VHCL] [varchar](50) NULL,
	[TOMEET] [varchar](50) NULL,
	[INTM] [char](5) NULL,
	[OTTM] [char](5) NULL,
	[PURPOSE] [varchar](50) NULL,
	[REMARK] [varchar](50) NULL,
	[VSIMG] [image] NULL,
	[EXTRA1] [varchar](50) NULL,
	[EXTRA2] [varchar](50) NULL,
	[EXTRA3] [varchar](50) NULL,
	[EXTRA4] [varchar](50) NULL,
	[EXTRA5] [varchar](50) NULL,
	[VID] [char](18) NULL,
	[MOB] [char](12) NULL,
	[VBNO] [char](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE dbo.VISITOR DROP COLUMN [Identity];
ALTER TABLE dbo.VISITOR ADD Id INT IDENTITY(1,1) NOT NULL;
ALTER TABLE dbo.VISITOR ADD CONSTRAINT PK_VISITOR_Id PRIMARY KEY (Id);

-- If VSIMG currently type image, you can keep it but recommended to convert:
-- BACKUP FIRST; converting may require creating a new column, copying, dropping old
ALTER TABLE dbo.VISITOR ALTER COLUMN VSIMG VARBINARY(MAX);

ALTER TABLE dbo.VISITOR ALTER COLUMN INTM datetime NULL;
ALTER TABLE dbo.VISITOR ALTER COLUMN OTTM datetime NULL;
