using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SpottedUnitn.Services.Dto.User
{
    [DataContract]
    public class AuthenticatedUserDto
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "role")]
        public string Role { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
