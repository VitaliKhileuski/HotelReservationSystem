namespace Business.Models
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Pagination()
        {
            PageNumber = 1;
            PageSize = 8;
        }
        public Pagination(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
        }
    }
}