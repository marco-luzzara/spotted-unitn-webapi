DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ProfilePhoto');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] DROP COLUMN [ProfilePhoto];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Shops]') AND [c].[name] = N'CoverPicture');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Shops] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Shops] DROP COLUMN [CoverPicture];

GO

ALTER TABLE [Shops] ADD [PhoneNumber] VARCHAR(15) NULL DEFAULT '';

GO

CREATE TABLE [ShopCoverPicture] (
    [ShopId] int NOT NULL,
    [CoverPicture] varbinary(max) NOT NULL DEFAULT 0x,
    CONSTRAINT [PK_ShopCoverPicture] PRIMARY KEY ([ShopId]),
    CONSTRAINT [FK_ShopCoverPicture_Shops_ShopId] FOREIGN KEY ([ShopId]) REFERENCES [Shops] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserProfilePhoto] (
    [UserId] int NOT NULL,
    [ProfilePhoto] varbinary(max) NOT NULL,
    CONSTRAINT [PK_UserProfilePhoto] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_UserProfilePhoto_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200914083015_SplitTableForImages', N'3.1.8');

GO

