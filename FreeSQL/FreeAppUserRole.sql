CREATE TABLE [dbo].[FreeAppUserRole]
(
	[RoleId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [PK_FreeAppUserRole] PRIMARY KEY CLUSTERED ([RoleId], [UserId]) 
)
