using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpottedUnitn.WebApi.Authorization
{
    public static class AuthorizationOptionsExtension
    {
        public const string onlyRegisteredOrAdminPolicy = "onlyRegisteredOrAdminPolicy";
        public const string onlyAdminPolicy = "onlyAdminPolicy";

        public static void AddOnlyRegisteredOrAdminPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(onlyRegisteredOrAdminPolicy, policyBuilder =>
            {
                policyBuilder
                    .RequireRole(UserRoleContainer.Registered, UserRoleContainer.Admin);
            });
        }

        public static void AddOnlyAdminPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(onlyAdminPolicy, policyBuilder =>
            {
                policyBuilder
                    .RequireRole(UserRoleContainer.Admin);
            });
        }
    }
}
