﻿using System;
using System.Collections.Generic;

namespace Business.Models
{
   public class OrderModel
   {
        public int Id { get; set; }
       public ICollection<ServiceModel> Services;
        public  UserModel Customer { get; set; }
        public RoomModel Room { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal FullPrice { get; set; }
    }
}