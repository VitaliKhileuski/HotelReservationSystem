using System;
using System.Text;
using Business;
using Business.Interfaces;
using Business.Mappers;
using Business.Services;
using HotelReservation.Api.Mappers;
using HotelReservation.Api.Policy;
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
            services.AddScoped<InitialData>();
            services.Configure<AuthOptions>(Configuration.GetSection(AuthOptions.Authentication));
            services.AddScoped<IPasswordHasher,PasswordHasher>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IBaseRepository<RoomEntity>, RoomRepository>();
            services.AddScoped<IBaseRepository<HotelEntity>, HotelRepository>();
            services.AddScoped<IBaseRepository<OrderEntity>, OrderRepository>();
            services.AddScoped<IBaseRepository<LocationEntity>, LocationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBaseRepository<ServiceEntity>, ServiceRepository>();

            services.AddScoped<MapConfiguration>();
            services.AddScoped<CustomMapperConfiguration>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRoomService,RoomsService>();
            services.AddScoped<IOrderService,OrdersService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddTransient<IUserService,UsersService>();
            services.AddScoped<IFacilityService,FacilitiesService>();
            services.AddScoped<LocationsService>();
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["AuthenticationOptions:issuer"],
                    ValidateIssuer = bool.Parse(Configuration["AuthenticationOptions:ValidateIssuer"] ?? "false"),
                    ValidAudience = Configuration["AuthenticationOptions:audience"],
                    ValidateAudience = bool.Parse(Configuration["AuthenticationOptions:ValidateAudience"] ?? "false"),
                    ValidateLifetime = bool.Parse(Configuration["AuthenticationOptions:ValidateLifetime"] ?? "false"),
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthenticationOptions:secretKey"])),
                    ValidateIssuerSigningKey = bool.Parse(Configuration["AuthenticationOptions:ValidateIssuerSigningKey"] ?? "false"),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminPermission, builder => builder.Combine(Policies.AdminPermissionPolicy()));
                options.AddPolicy(Policies.HotelAdminPermission, builder => builder.Combine(Policies.HotelAdminPermissionPolicy()));
                options.AddPolicy(Policies.AllAdminsPermission, builder => builder.Combine(Policies.AllAdminsPermissionPolicy()));
                options.AddPolicy(Policies.UserPermission, builder => builder.Combine(Policies.UserPermissionPolicy()));
            });
            services.AddCors(options =>
            {
                options.AddPolicy(Policies.ApiCors, builder => Policies.ApiCorsPolicy(builder));
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
            app.UseCors(builder => Policies.ApiCorsPolicy(builder));
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
            var contextInitializer = serviceScope.ServiceProvider.GetService<InitialData>();
            contextInitializer?.InitializeContext();
        }
    }
}