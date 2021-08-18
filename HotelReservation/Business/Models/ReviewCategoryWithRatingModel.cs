using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Generators;

namespace Business.Models
{
    public class ReviewCategoryWithRatingModel
    {
        public ReviewCategoryModel Category { get; set; }
        public double Rating { get; set; }
    }
}
