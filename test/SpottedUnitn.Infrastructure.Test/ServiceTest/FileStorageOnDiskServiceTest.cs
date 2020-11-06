using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Services.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Infrastructure.Test.ServiceTest
{
    [TestClass]
    public class FileStorageOnDiskServiceTest
    {
        protected readonly string TEMPDIR = "temp_dir_for_testing";

        [DataTestMethod]
        [DataRow("test")]
        [DataRow("")]
        public async Task StoreRetrieveAndDeleteFile_ValidFile_Ok(string fileContentStr)
        {
            DirectoryInfo dirInfo = null;
            var fileName = "user_1";
            var fileContent = Encoding.UTF8.GetBytes(fileContentStr);
            var fileStorageService = new FileStorageOnDiskService(new FileStorageOnDiskService.FileStorageOnDiskServiceConfig()
            {
                BasePath = TEMPDIR
            });

            try
            {
                dirInfo = Directory.CreateDirectory(TEMPDIR);
                // first store
                await fileStorageService.StoreFileAsync(fileName, fileContent);
                var filePath = Path.Join(TEMPDIR, fileName);

                Assert.IsTrue(File.Exists(filePath));

                var fileContentRet = await fileStorageService.GetFileAsync(fileName);
                CollectionAssert.AreEqual(fileContent, fileContentRet);

                // second store
                var newFileContent = fileContent.Concat(new byte[] { 0x00 }).ToArray();
                await fileStorageService.StoreFileAsync(fileName, newFileContent);

                Assert.IsTrue(File.Exists(filePath));

                fileContentRet = await fileStorageService.GetFileAsync(fileName);
                CollectionAssert.AreEqual(newFileContent, fileContentRet);

                await fileStorageService.DeleteFileAsync(fileName);
                Assert.IsFalse(File.Exists(filePath));
            }
            finally
            {
                if (dirInfo != null)
                    Directory.Delete(TEMPDIR, true);
            }
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task StoreFile_InvalidId_Throw(string fileId)
        {
            var fileStorageService = new FileStorageOnDiskService(new FileStorageOnDiskService.FileStorageOnDiskServiceConfig()
            {
                BasePath = TEMPDIR
            });

            await fileStorageService.StoreFileAsync(fileId, new byte[] { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task StoreFile_InvalidFileContent_Throw()
        {
            var fileStorageService = new FileStorageOnDiskService(new FileStorageOnDiskService.FileStorageOnDiskServiceConfig()
            {
                BasePath = TEMPDIR
            });

            await fileStorageService.StoreFileAsync("user_1", null);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFile_InvalidId_Throw(string fileId)
        {
            var fileStorageService = new FileStorageOnDiskService(new FileStorageOnDiskService.FileStorageOnDiskServiceConfig()
            {
                BasePath = TEMPDIR
            });

            await fileStorageService.GetFileAsync(fileId);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task DeleteFile_InvalidId_Throw(string fileId)
        {
            var fileStorageService = new FileStorageOnDiskService(new FileStorageOnDiskService.FileStorageOnDiskServiceConfig()
            {
                BasePath = TEMPDIR
            });

            await fileStorageService.DeleteFileAsync(fileId);
        }
    }
}
