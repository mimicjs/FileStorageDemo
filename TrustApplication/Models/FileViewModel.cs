using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrustApplication.Models
{
    public class FileViewModel
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        public DateTime StoredDateTime { get; set; }

        public string Content { get; set; }
    }
}
