using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Exceptions;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;

namespace Business.Helpers
{
    public static class PermissionVerifier
    {
        public static bool CheckPermission(HotelEntity hotel, UserEntity user)
        {
            if (hotel.Admins.FirstOrDefault(x => x.Id == user.Id) != null || user.Role.Name == Roles.Admin)
            {
                return true;
            }
            else
            {
                throw new BadRequestException($"user with {user.Id}  don't have permission to manage hotel with {hotel.Id} id");
            }
        }
    }
}
