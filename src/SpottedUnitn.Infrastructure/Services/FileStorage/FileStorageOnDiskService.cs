using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpottedUnitn.Infrastructure.Services.FileStorage
{
    public class FileStorageOnDiskService : IFileStorageService
    {
        protected FileStorageOnDiskServiceConfig storageConfig;

        public FileStorageOnDiskService(FileStorageOnDiskServiceConfig config)
        {
            this.storageConfig = config;
        }

        protected bool IsFileIdValid(string fileId)
        {
            return !string.IsNullOrEmpty(fileId);
        }

        protected string BuildFilePath(string fileId)
        {
            return Path.Join(this.storageConfig.BasePath, fileId);
        }

        public async Task<byte[]> GetFileAsync(string fileId, CancellationToken ct = default)
        {
            if (!IsFileIdValid(fileId))
                throw new ArgumentException("invalid fileId, cannot be empty or null");

            string filePath = BuildFilePath(fileId);

            return await File.ReadAllBytesAsync(filePath, ct);
        }

        public async Task StoreFileAsync(string fileId, byte[] fileContent, CancellationToken ct = default)
        {
            if (!IsFileIdValid(fileId))
                throw new ArgumentException("invalid fileId: cannot be empty or null");

            if (fileContent is null)
                throw new ArgumentNullException("invalid fileContent: cannot be null");

            string filePath = BuildFilePath(fileId);

            if (fileContent.Length == 0)
                await File.Create(filePath).DisposeAsync();
            else
                await File.WriteAllBytesAsync(filePath, fileContent, ct);
        }

        public Task DeleteFileAsync(string fileId, CancellationToken ct = default)
        {
            if (!IsFileIdValid(fileId))
                throw new ArgumentException("invalid fileId: cannot be empty or null");

            string filePath = BuildFilePath(fileId);
            File.Delete(filePath);

            return Task.CompletedTask;
        }

        public class FileStorageOnDiskServiceConfig
        {
            public string BasePath { get; set; }
        }
    }
}
