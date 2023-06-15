USE master
GO
IF NOT EXISTS (
   SELECT name
   FROM sys.databases
   WHERE name = N'PictureDB'
)
CREATE DATABASE [PictureDB]
GO
    
USE [PictureDB]
-- Create a new table called 'Images' in schema 'dbo'
-- Drop the table if it already exists
IF OBJECT_ID('dbo.Images', 'U') IS NOT NULL
DROP TABLE dbo.Images
    GO
-- Create the table in the specified schema
CREATE TABLE dbo.Images
(
    Id        INT    NOT NULL IDENTITY(1,1)  PRIMARY KEY, -- primary key column
    Name      [NVARCHAR](45)  NOT NULL,
    Url       [NVARCHAR](45)  NOT NULL
);
GO    


USE [PictureDB]
GO

INSERT INTO [dbo].[Images]
           ([Name]
           ,[Url])
     VALUES
           ('Cardinal','4006')
GO
  