using System.Linq;
using Business.Exceptions;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;

namespace Business.Helpers
{
    public static class PermissionVerifier
    {
        public static bool CheckHotelPermission(HotelEntity hotel, UserEntity user)
        {
            if (hotel.Admins.FirstOrDefault(x => x.Id == user.Id) != null || user.Role.Name == Roles.Admin)
            {
                return true;
            }

            throw new BadRequestException($"user with {user.Id}  don't have permission to manage hotel with {hotel.Id} id");
        }

        public static bool CheckUserPermission(UserEntity user, string userId)
        {
            if (user.Id.ToString() == userId)
            {
                return true;
            }

            throw new BadRequestException("you don't have permissions to do actions with this user");
        }
    }
}