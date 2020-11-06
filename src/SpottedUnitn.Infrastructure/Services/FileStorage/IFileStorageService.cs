using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpottedUnitn.Infrastructure.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<byte[]> GetFileAsync(string fileId, CancellationToken ct = default);

        // store and overwrite if already exists
        Task StoreFileAsync(string fileId, byte[] fileContent, CancellationToken ct = default);

        Task DeleteFileAsync(string fileId, CancellationToken ct = default);
    }
}
