using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class HotelPagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public HotelPagination()
        {
            PageNumber = 1;
            PageSize = 8;
        }
        public HotelPagination(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
        }
    }
}
