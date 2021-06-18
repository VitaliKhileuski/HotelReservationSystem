using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelReservation.Api.Extensions
{
    public static class ConfigureAuthenticationExtensions
    {
        public static void AddTokenAuthentication(this IServiceCollection services, AuthOptions authenticationOptions)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authenticationOptions.Issuer,
                        ValidateIssuer = authenticationOptions.ValidateIssuer,
                        ValidAudience = authenticationOptions.Audience,
                        ValidateAudience = authenticationOptions.ValidateAudience,
                        ValidateLifetime = authenticationOptions.ValidateLifetime,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.SecretKey)),
                        ValidateIssuerSigningKey = authenticationOptions.ValidateIssuerSigningKey,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
