using System.Collections.Generic;

namespace Business.Models
{
    public class PageInfo<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int NumberOfItems { get; set; }
        public int NumberOfPages { get; set; }
    }
}