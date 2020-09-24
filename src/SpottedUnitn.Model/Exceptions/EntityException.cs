using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool HasCodeIn(params int[] codes)
        {
            return codes.Contains(this.Code);
        }
    }
}
