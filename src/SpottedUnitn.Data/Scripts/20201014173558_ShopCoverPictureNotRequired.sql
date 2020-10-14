DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ShopCoverPicture]') AND [c].[name] = N'CoverPicture');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ShopCoverPicture] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ShopCoverPicture] ALTER COLUMN [CoverPicture] varbinary(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201014173558_ShopCoverPictureNotRequired', N'3.1.8');

GO

