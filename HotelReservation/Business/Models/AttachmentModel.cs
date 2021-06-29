using System;

namespace Business.Models
{
    public class AttachmentModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public FileContentModel FileContent { get; set; }

    }
}