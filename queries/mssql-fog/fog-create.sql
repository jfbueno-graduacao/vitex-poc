CREATE DATABASE VitalSignReadings;

USE VitalSignReadings;

DROP TABLE IF EXISTS [Temperature];

CREATE TABLE [Temperature]
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	PersonId UNIQUEIDENTIFIER NOT NULL,
	[Value] DECIMAL(3, 1) NOT NULL,
	[Timestamp] DATETIME2 NOT NULL,

	ReadForIntegration BIT NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS [FogNodeMetadata];
CREATE TABLE [FogNodeMetadata]
(
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[Name] VARCHAR(MAX) NOT NULL,
);

DROP TABLE IF EXISTS [People];
CREATE TABLE [People]
(
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
	BirthDate DATETIME2 NOT NULL,
	BaseTemperature DECIMAL(3, 1) NOT NULL
);
