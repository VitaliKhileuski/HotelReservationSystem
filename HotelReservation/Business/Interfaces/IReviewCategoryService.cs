using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IReviewCategoryService
    {
        Task CreateReviewCategory(ReviewCategoryModel reviewCategory);

        IEnumerable<ReviewCategoryModel> GetAllReviewCategories();
        Task DeleteReviewCategory(Guid reviewCategoryId);
    }
}
