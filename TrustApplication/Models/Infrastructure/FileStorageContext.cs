using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrustApplication.Models.Infrastructure
{
    public class FileStorageContext : DbContext
    {
        public FileStorageContext(DbContextOptions<FileStorageContext> options) : base(options)
        {

        }

        public DbSet<FileEntity> Files { get; set; }
    }
}
