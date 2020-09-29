using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public abstract class EntityException : Exception
    {
        public int Code { get; set; }

        public object[] MessageParams { get; set; }

        // message is the entire message
        public override string Message => string.Format(this.FormattedMessage, this.MessageParams);

        // formattedMessage is the message with parameters unfilled
        public string FormattedMessage { get; }

        public EntityException(int code, string message, params object[] messageParams) : base()
        {
            this.Code = code;
            this.FormattedMessage = message;
            this.MessageParams = messageParams;
        }

        public bool HasCodeIn(params int[] codes)
        {
            return codes.Contains(this.Code);
        }

        public abstract string GetCodeName();
    }
}
