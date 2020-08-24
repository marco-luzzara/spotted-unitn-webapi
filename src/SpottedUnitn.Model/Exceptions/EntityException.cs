using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class EntityException : Exception
    {
        public int Code { get; set; }

        public EntityException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
