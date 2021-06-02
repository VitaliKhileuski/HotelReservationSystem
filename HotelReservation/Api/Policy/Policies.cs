﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace HotelReservation.Api.Policy
{
    public static class Policies
    {
        public const string AdminPermission = "Admin";

        public const string HotelAdminPermission = "HotelAdmin";

        public const string UserPermission = "User";

        public const string AllAdminsPermission = AdminPermission + "," + HotelAdminPermission;

        public const string ApiCors = "ApiCorsPolicy";

        private const string FrontLocalHost = "http://localhost:3000";
        public static AuthorizationPolicy AdminPermissionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser() 
                .RequireRole(AdminPermission)
                .Build(); 
        }

        public static AuthorizationPolicy HotelAdminPermissionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole(HotelAdminPermission)
                .Build();
        }

        public static AuthorizationPolicy UserPermissionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole(UserPermission)
                .Build();
        }

        public static AuthorizationPolicy AllAdminsPermissionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireRole(AllAdminsPermission)
                .Build();
        }

        public static CorsPolicyBuilder ApiCorsPolicy()
        {
            return new CorsPolicyBuilder()
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
               
        }
    }
}
