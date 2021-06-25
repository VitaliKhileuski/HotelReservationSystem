using System;

namespace Business.Models
{
    public class AttachmentModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public FileContentModel Content { get; set; }

    }
}