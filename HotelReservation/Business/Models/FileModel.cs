using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    class FileModel
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set;}
    }
}
