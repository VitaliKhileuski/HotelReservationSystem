using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IReviewCategoryRepository : IBaseRepository<ReviewCategoryEntity>
    {
        Task<ReviewCategoryEntity> GetReviewCategoryByName(string name);
    }
}
