using Business.Interfaces;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.Api.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRoomService, RoomsService>();
            services.AddScoped<IOrderService, OrdersService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IAttachmentsService, AttachmentsService>();
            services.AddScoped<IFacilityService, FacilitiesService>();
            services.AddScoped<LocationsService>();
        }
    }
}