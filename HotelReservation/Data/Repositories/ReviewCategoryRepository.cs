using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ReviewCategoryRepository : BaseRepository<ReviewCategoryEntity>, IReviewCategoryRepository
    {
        private readonly Context _db;

        public ReviewCategoryRepository(Context context) : base(context)
        {
            _db = context;
        }

        public async Task<ReviewCategoryEntity> GetReviewCategoryByName(string name)
        {
            return await _db.ReviewCategories.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
