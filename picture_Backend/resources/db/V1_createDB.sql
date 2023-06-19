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
    Name      [NVARCHAR](100)  NOT NULL,
    Url       [NVARCHAR](1000)  NOT NULL
);
GO    


USE [PictureDB]
GO

INSERT INTO [dbo].[Images]
           ([Name]
           ,[Url])
     VALUES
           ('169512d7-d92a-4ee0-877c-ad7cf8509832_zdhzh','wwwroot\images\AA151hvS.jpg')
GO
  