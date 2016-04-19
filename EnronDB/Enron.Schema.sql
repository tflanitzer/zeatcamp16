﻿DROP TABLE Sender
DROP TABLE Recipient
DROP TABLE Attachment
DROP TABLE Header
DROP TABLE WordOccurrence
DROP TABLE Mail
DROP TABLE EmailAccount
DROP TABLE Word

CREATE TABLE [dbo].[EmailAccount]
(
	[Id] INT NOT NULL PRIMARY KEY,
	EmailAddress NVARCHAR(250) NOT NULL,
	CONSTRAINT UNIQUE_EmailAddress UNIQUE(EmailAddress)
)

CREATE TABLE [dbo].Mail
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Subject] NVARCHAR(MAX) NOT NULL,
	[Date] BIGINT NOT NULL,
	[Body] NVARCHAR(MAX) NOT NULL,
	[SubFolder] NVARCHAR(MAX) NULL,
	[OriginalId] NVARCHAR(MAX) NULL
)

CREATE TABLE [dbo].Word
(
	[Id] INT NOT NULL PRIMARY KEY,
	Word NVARCHAR(MAX) NOT NULL
)

CREATE TABLE [dbo].Sender
(
	MailId INT NOT NULL PRIMARY KEY,
	EmailAccountId INT NOT NULL FOREIGN KEY REFERENCES EmailAccount(Id),
	Name NVARCHAR(MAX) NULL,
	FOREIGN KEY (MailId) REFERENCES Mail(Id)
)

CREATE TABLE [dbo].Recipient
(
	[Id] INT NOT NULL PRIMARY KEY,
	EmailAccountId INT NOT NULL FOREIGN KEY REFERENCES EmailAccount(Id),
	MailId INT NOT NULL FOREIGN KEY REFERENCES Mail(Id),
	Name INT NOT NULL,
	[Type] NVARCHAR(3) NOT NULL
)

CREATE TABLE [dbo].Attachment
(
	[Id] INT NOT NULL PRIMARY KEY,
	MailId INT NOT NULL FOREIGN KEY REFERENCES Mail(Id),
	Name NVARCHAR(MAX) NOT NULL
)

CREATE TABLE [dbo].Header
(
	[Id] INT NOT NULL PRIMARY KEY,
	MailId INT NOT NULL FOREIGN KEY REFERENCES Mail(Id),
	[Key] NVARCHAR(100) NOT NULL,
	[Value] NVARCHAR(MAX) NULL
)

CREATE TABLE [dbo].WordOccurrence
(
	[Id] INT NOT NULL PRIMARY KEY,
	MailId INT NOT NULL FOREIGN KEY REFERENCES Mail(Id),
	WordId INT NOT NULL FOREIGN KEY REFERENCES Word(Id),
	Position INT NULL
)

