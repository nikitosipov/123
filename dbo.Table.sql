CREATE TABLE [dbo].[Table1]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nomer] NVARCHAR(50) NULL, 
    [Narushenie] NVARCHAR(50) NULL, 
    [Data] DATETIME NULL, 
    [Time] DATETIME2 NULL, 
    [Shtraf] INT NULL, 
)
