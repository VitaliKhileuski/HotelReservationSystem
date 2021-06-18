using System;
using System.Text;
using Business;
using Business.Interfaces;
using Business.Mappers;
using Business.Services;
using HotelReservation.Api.Extensions;
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
            ConfigureLogger();
        }

        public IConfiguration Configuration { get; }

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

            services.AddRepositories();
            services.AddServices();

            services.AddScoped<MapConfiguration>();
            services.AddScoped<CustomMapperConfiguration>();

           
            services.AddControllers();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
            services.AddTokenAuthentication(Configuration.GetSection(AuthOptions.Authentication).Get<AuthOptions>());
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
            app.UseCors(builder => Policies.ApiCorsPolicy(builder));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
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
        public static void ConfigureLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}