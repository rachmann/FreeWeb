CREATE TABLE [dbo].[FreeAppUsers]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[UserId] INT NOT NULL, 
    [Description] NVARCHAR(200) NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [EmailConfirmed] BIT NOT NULL, 
    [PasswordHash] NVARCHAR(500) NOT NULL, 
    [SecurityStamp] NVARCHAR(50) NOT NULL, 
    [PhoneNumber] VARCHAR(20) NOT NULL, 
    [PhoneNumberConfirmed] BIT NOT NULL, 
    [TwoFactorEnabled] BIT NOT NULL, 
    [LockoutEndDateUtc] DATETIME NULL, 
    [LockoutEnabled] BIT NOT NULL, 
    [AccessFailedCount] INT NOT NULL, 
    [ApplicationName] NCHAR(10) NOT NULL,
	CONSTRAINT [PK_FreeAppUser] PRIMARY KEY CLUSTERED ([Id] ASC)

)
