IF NOT EXISTS (SELECT * FROM master.sys.databases WHERE name = 'independent_financial')
BEGIN
    CREATE DATABASE independent_financial;
END
GO

USE independent_financial;

CREATE TABLE [Employee] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [user] varchar(100) NOT NULL,
  [password] varchar(1000) NOT NULL,
  [name] varchar(100) NOT NULL,
  [mail] varchar(100) NOT NULL,
  [address] varchar(400) NOT NULL,
  [phoneNumber] char(10) NOT NULL,
  [birthday] Date NOT NULL,
  [role] VARCHAR(9) NOT NULL CHECK (role in ('analist','admin','adviser','collector')),
  [sucursalId] int NOT NULL
)
GO

CREATE TABLE [Subsidiary] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [Address] varchar(400) NOT NULL,
  [state] bit NOT NULL
)
GO

CREATE TABLE [Credit] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [state] VARCHAR(14) NOT NULL CHECK (state in ('requested','rejected','active','not_chargeable','ended')),
  [duration] tinyint NOT NULL,
  [capital] int NOT NULL,
  [beneficiary] char(13) NOT NULL,
  [registrer] int NOT NULL,
  [conditionId] int NOT NULL
)
GO

CREATE TABLE [Document] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] varchar(100) NOT NULL,
  [registryDate] DateTime NOT NULL,
  [registrer] int NOT NULL,
  [documentationId] int NOT NULL,
  [creditId] int NOT NULL
)
GO

CREATE TABLE [RequiredDocumentation] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] varchar(50) NOT NULL,
  [state] BIT NOT NULL,
  [fileType] VARCHAR(5) NOT NULL CHECK (fileType in ('image','pdf'))
)
GO

CREATE TABLE [Client] (
  [rfc] char(13) PRIMARY KEY NOT NULL,
  [name] varchar(100) NOT NULL,
  [birthday] Date NOT NULL,
  [houseAddress] varchar(400) NOT NULL,
  [workAddress] varchar(400) NOT NULL,
  [phoneNumber1] char(10) NOT NULL,
  [phoneNumber2] char(10) NOT NULL,
  [mail] varchar(100) NOT NULL,
  [state] bit NOT NULL
)
GO

CREATE TABLE [BankAccount] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [clabe] char(18) NOT NULL,
  [purpose] VARCHAR(7) NOT NULL CHECK (purpose in ('receive','collect')),
  [clientId] char(13) NOT NULL
)
GO

CREATE TABLE [Payment] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [collectionDate] Date NOT NULL,
  [amount] DECIMAL(7,2) NOT NULL,
  [state] VARCHAR(13) NOT NULL CHECK (state in ('collected','pending','not_collected')),
  [registrer] int NOT NULL,
  [creditId] int NOT NULL
)
GO

CREATE TABLE [CreditCondition] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [state] bit NOT NULL,
  [interestRate] int NOT NULL,
  [IVA] int NOT NULL,
  [paymentsPerMonth] int NOT NULL,
  [registrer] int NOT NULL
)
GO

CREATE TABLE [CreditPolicy] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [state] bit NOT NULL,
  [title] varchar(50) NOT NULL,
  [description] varchar(100) NOT NULL,
  [effectiveDate] Date NOT NULL,
  [registrer] int NOT NULL
)
GO

ALTER TABLE [Employee] ADD FOREIGN KEY ([sucursalId]) REFERENCES [Subsidiary] ([id])
GO

ALTER TABLE [Credit] ADD FOREIGN KEY ([beneficiary]) REFERENCES [Client] ([rfc])
GO

ALTER TABLE [Credit] ADD FOREIGN KEY ([registrer]) REFERENCES [Employee] ([id])
GO

ALTER TABLE [Credit] ADD FOREIGN KEY ([conditionId]) REFERENCES [CreditCondition] ([id])
GO

ALTER TABLE [Document] ADD FOREIGN KEY ([registrer]) REFERENCES [Employee] ([id])
GO

ALTER TABLE [Document] ADD FOREIGN KEY ([documentationId]) REFERENCES [RequiredDocumentation] ([id])
GO

ALTER TABLE [Document] ADD FOREIGN KEY ([creditId]) REFERENCES [Credit] ([id])
GO

ALTER TABLE [BankAccount] ADD FOREIGN KEY ([clientId]) REFERENCES [Client] ([rfc])
GO

ALTER TABLE [Payment] ADD FOREIGN KEY ([registrer]) REFERENCES [Employee] ([id])
GO

ALTER TABLE [Payment] ADD FOREIGN KEY ([creditId]) REFERENCES [Credit] ([id])
GO

ALTER TABLE [CreditCondition] ADD FOREIGN KEY ([registrer]) REFERENCES [Employee] ([id])
GO

ALTER TABLE [CreditPolicy] ADD FOREIGN KEY ([registrer]) REFERENCES [Employee] ([id])
GO

--Create users for every role
--:r user-creation.sql
--GO

--Charge store procedures to the database
--:r stored-procedures.sql
--GO

--Assign permissions to every role for esecut stored procedures
--:r stored-procedures-permissions.sql
--GO
