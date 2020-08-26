using SpottedUnitn.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Test.Mocks.Services
{
    public class DateTimeOffsetServiceMock : IDateTimeOffsetService
    {
        private DateTimeOffset now;
        public DateTimeOffset Now => this.now;

        private DateTimeOffset utcNow;
        public DateTimeOffset UtcNow => this.utcNow;

        public DateTimeOffsetServiceMock(DateTimeOffset dtoNow, DateTimeOffset dtoUtcNow = default)
        {
            this.now = dtoNow;
            this.utcNow = dtoUtcNow == default ? DateTimeOffset.UtcNow : dtoUtcNow;
        }
    }
}
