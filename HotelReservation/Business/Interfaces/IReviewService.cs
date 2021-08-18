using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IReviewService : IReviewCategoryService
    {
        Task CreateReview(Guid orderId, string userId, ReviewModel review);
    }
}
