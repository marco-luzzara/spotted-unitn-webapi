using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using static SpottedUnitn.Model.UserAggregate.User;

namespace SpottedUnitn.Data.Dto.User
{
    [DataContract]
    public class LoggedInUser
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "role")]
        public UserRole Role { get; set; }
    }
}
