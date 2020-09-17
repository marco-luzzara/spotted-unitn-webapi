using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Dto.User
{
    public class UserBasicInfoDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Mail { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTimeOffset? ExpirationDate{ get; set; }
    }
}
