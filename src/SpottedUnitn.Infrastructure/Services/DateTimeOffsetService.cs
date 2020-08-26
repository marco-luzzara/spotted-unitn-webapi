using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Services
{
    public class DateTimeOffsetService : IDateTimeOffsetService
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
