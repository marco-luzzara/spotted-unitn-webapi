IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Shops] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [LinkToSite] nvarchar(max) NULL DEFAULT N'',
    [Description] nvarchar(max) NOT NULL,
    [Location_Address] nvarchar(max) NULL,
    [Location_City] nvarchar(max) NULL,
    [Location_Province] nvarchar(max) NULL,
    [Location_PostalCode] nvarchar(16) NULL,
    [Location_Latitude] real NULL,
    [Location_Longitude] real NULL,
    [CoverPicture] varbinary(max) NULL DEFAULT 0x,
    [Discount] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Shops] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Role] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Credentials_Mail] NVARCHAR(320) NULL,
    [Credentials_HashedPwd] VARCHAR(72) NULL,
    [ProfilePhoto] varbinary(max) NOT NULL,
    [SubscriptionDate] datetimeoffset NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE UNIQUE INDEX [IX_Credentials_Email] ON [Users] ([Credentials_Mail]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200909173127_Init', N'3.1.8');

GO

