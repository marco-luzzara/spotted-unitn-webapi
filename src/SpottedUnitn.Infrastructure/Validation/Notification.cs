using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Validation
{
    public class Notification
    {
        public List<string> Errors { get; set; }

        public Notification()
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
