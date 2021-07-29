using System;
using System.Collections.Generic;

namespace Business.Models
{
   public class OrderModel
   {
        public Guid Id { get; set; }
        public ICollection<ServiceQuantityModel> Services;
        public string UserEmail { get; set; }
        public UserModel Customer { get; set; }
        public string Number { get; set; }
        public RoomModel Room { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal FullPrice { get; set; }
    }
}