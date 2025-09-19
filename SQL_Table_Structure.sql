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
ALTER TABLE Visitor ADD VisitorCardId INT NULL;
ALTER TABLE Visitor 
ADD CONSTRAINT FK_Visitor_VisitorCard FOREIGN KEY (VisitorCardId) 
REFERENCES VisitorCard(Id);

CREATE TABLE VisitorCard (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CardNumber VARCHAR(20) NOT NULL UNIQUE,
    IsAssigned BIT NOT NULL DEFAULT 0
);

CREATE UNIQUE INDEX UQ_VISITOR_ActiveCard
ON VISITOR (VisitorCardId)
WHERE OTTM IS NULL;

ALTER DATABASE [bppdb] SET ARITHABORT ON;

INSERT INTO VisitorCard (CardNumber) VALUES ('C001'), ('C002'), ('C003'), ('C004');

select * from VisitorCard
SELECT * FROM VISITOR