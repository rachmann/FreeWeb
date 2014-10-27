CREATE TABLE [dbo].[FreeAppUserClaims]
(
	[ClaimId] INT IDENTITY(1,1) NOT NULL, 
    [UserId] INT NOT NULL, 
    [ClaimTypeId] INT NOT NULL, 
    [ClaimValue] VARCHAR(10) NOT NULL, 
    [ClaimValueType] VARCHAR(10) NOT NULL, 
    [Issuer] NVARCHAR(200) NOT NULL, 
	CONSTRAINT [PK_FreeAppUserClaim] PRIMARY KEY CLUSTERED ([ClaimId] ASC),
	CONSTRAINT [FK_dbo.FreeAppUserClaim_dbo.FreeAppUser_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[FreeAppUsers] ([Id]) ON DELETE CASCADE
)
