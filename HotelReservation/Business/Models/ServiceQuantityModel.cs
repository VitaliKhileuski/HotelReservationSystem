using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ServiceQuantityModel
    {
        public Guid Id { get; set; }
        public  ServiceModel Service { get; set; }
        public int Quantity { get; set; }
        public  OrderModel Order { get; set; }
    }
}
