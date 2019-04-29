﻿CREATE TABLE [dbo].[Recipes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(100) NOT NULL, 
    [Instructions] VARCHAR(1000) NULL, 
    [UserId] BIGINT NOT NULL, 
    CONSTRAINT [FK_Recipes_ToUsers] FOREIGN KEY (UserId) REFERENCES Users(Id)
)
