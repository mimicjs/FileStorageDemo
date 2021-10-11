using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrustApplication.Models.Infrastructure
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly FileStorageContext _db;

        public FileStorageRepository(FileStorageContext db)
        {
            _db = db;
        }
        public IQueryable<FileEntity> Files => _db.Files;

        public void Add<EntityType>(EntityType entity) => _db.Add(entity);
        public void SaveChanges() => _db.SaveChanges();
    }
}
