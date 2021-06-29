using System.Collections.Generic;
using System.Linq;
using Business.Models;

namespace HotelReservation.Api.Helpers
{
    public class UrlHelper
    {
        private const string BaseImageUrl = "https://localhost:5001/api/images/";
        public static ICollection<string> GetUrls(IEnumerable<AttachmentModel> attachments)
        {
            return attachments.Select(attachment => BaseImageUrl + attachment.Id).ToList();
        }
    }
}