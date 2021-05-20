using System;
using System.Text;
using Business.Interfaces;
using Business.Mappers;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace HotelReservation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseSqlServer(Configuration.GetConnectionString("HotelContextConnection"),
                    x => x.MigrationsAssembly("Api"));
            });
            services.AddScoped<HashPassword>();
            services.AddScoped<RoomRepository>();
            services.AddScoped<HotelRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ServiceRepository>();

            services.AddScoped<MapConfiguration>();
            services.AddScoped<CustomMapperConfiguration>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRoomService,RoomsService>();
            services.AddScoped<IOrderService,OrdersService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IUserService,UsersService>();
            services.AddScoped<IFacilityService,FacilitiesService>();
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = bool.Parse(Configuration["AuthenticationOptions:RequireHttpsMetadata"] ?? "false");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["AuthenticationOptions:issuer"],
                    ValidateIssuer = bool.Parse(Configuration["AuthenticationOptions:ValidateIssuer"] ?? "false"),
                    ValidAudience = Configuration["AuthenticationOptions:audience"],
                    ValidateAudience = bool.Parse(Configuration["AuthenticationOptions:ValidateAudience"] ?? "false"),
                    ValidateLifetime = bool.Parse(Configuration["AuthenticationOptions:ValidateLifetime"] ?? "false"),
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secrets:secretKey"])),
                    ValidateIssuerSigningKey = bool.Parse(Configuration["AuthenticationOptions:ValidateIssuerSigningKey"] ?? "false"),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminPermission", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });
                opt.AddPolicy("HotelAdminPermission", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin", "HotelAdmin");
                });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "ApiCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("ApiCorsPolicy");

            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            Migrate(app);
        }

        private void Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<Context>();
            context?.Database.Migrate();
        }
    }
}