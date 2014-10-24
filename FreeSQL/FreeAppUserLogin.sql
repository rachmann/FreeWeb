CREATE TABLE [dbo].[FreeAppUserLogin]
(
	[UserId] INT NOT NULL , 
    [LoginProvider] NVARCHAR(500) NOT NULL, 
    [ProviderKey] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [PK_FreeAppUserLogin] PRIMARY KEY CLUSTERED ([UserId], [ProviderKey], [LoginProvider] ASC) ,
	CONSTRAINT [FK_dbo.FreeAppUserLogin_dbo.FreeAppUser_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[FreeAppUser] ([Id]) ON DELETE CASCADE

)
