using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Services
{
    public interface IDateTimeOffsetService
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}
