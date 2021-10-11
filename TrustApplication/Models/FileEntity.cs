using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrustApplication.Models
{
    public class FileEntity
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        private DateTime storedDateTime;
        public DateTime StoredDateTime 
        { 
            get { return storedDateTime; } 
            set { storedDateTime = DateTime.UtcNow; } 
        }

        public string Content { get; set; }

        //In future I'd store content in the cloud (e.g. S3 and GUID the filename), store the GUID here

    }
}
