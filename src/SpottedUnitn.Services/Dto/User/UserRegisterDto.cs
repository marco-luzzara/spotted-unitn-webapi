using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Services.Dto.User
{
    public class UserRegisterDto
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public IFormFile ProfilePhoto { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }
    }
}
