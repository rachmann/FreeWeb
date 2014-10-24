CREATE TABLE [dbo].[FreeAppUserClaimType]
(
	[TypeId] INT IDENTITY(1,1) NOT NULL, 
    [ClaimTypeCode] VARCHAR(10) NOT NULL, 
    [ClaimTypeDescription] NVARCHAR(200) NOT NULL,
	CONSTRAINT [PK_FreeAppUserClaimType] PRIMARY KEY CLUSTERED ([TypeId] ASC)
)
