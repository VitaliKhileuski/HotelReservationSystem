﻿namespace HotelReservation.Api.Models.RequestModels
{
    public class RegisterUserRequestModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}