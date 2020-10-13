using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Infrastructure.Conversions
{
    public static class IFormFileExtension
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
        {
            if (formFile == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
