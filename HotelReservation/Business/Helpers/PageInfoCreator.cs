using System.Collections.Generic;
using System.Linq;
using Business.Models;

namespace Business.Helpers
{
    public static class PageInfoCreator<T>
    {
        public static PageInfo<T> GetPageInfo(ICollection<T> items,Pagination paginationInfo)
        {
          int pages = items.Count / paginationInfo.PageSize;
            if (items.Count % paginationInfo.PageSize != 0)
            {
                pages++;
            }
            var pagedData = items
                .Skip((paginationInfo.PageNumber - 1) * paginationInfo.PageSize)
                .Take(paginationInfo.PageSize)
                .ToList();
            var numberOfItems = items.Count;
            var hotelPageInfo = new PageInfo<T>
            {
                Items = pagedData,
                NumberOfItems = numberOfItems,
                NumberOfPages = pages
            };
            return hotelPageInfo;
        }
    }
}