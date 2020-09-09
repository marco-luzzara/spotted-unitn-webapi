using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Test.TestingUtility
{
    public class DataGenerator
    {
        public static string GenerateMail()
        {
            return $"{DateTimeOffset.Now:yyyyMMdd_hhmmss_fff}@test.it";
        }
    }
}
