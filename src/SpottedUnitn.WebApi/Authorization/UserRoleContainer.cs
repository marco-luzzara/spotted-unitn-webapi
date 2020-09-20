using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpottedUnitn.WebApi.Authorization
{
    public class UserRoleContainer
    {
        public const string Admin = nameof(User.UserRole.Admin);
        public const string Registered = nameof(User.UserRole.Registered);
    }
}
