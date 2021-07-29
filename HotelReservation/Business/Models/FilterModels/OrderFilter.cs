using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models.FilterModels
{
    public class OrderFilter
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
    }
}
