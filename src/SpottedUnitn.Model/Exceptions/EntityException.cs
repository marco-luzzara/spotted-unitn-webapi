using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class EntityException : Exception
    {
        public int Code { get; set; }

        public object[] MessageParams { get; set; }

        public EntityException(int code, string message, params object[] messageParams) : base(message)
        {
            this.Code = code;
            this.MessageParams = messageParams;
        }

        public bool HasCodeIn(params int[] codes)
        {
            return codes.Contains(this.Code);
        }
    }
}
