using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrustApplication.Models.Infrastructure
{
    public interface IFileStorageRepository
    {
        public IQueryable<FileEntity> Files { get; }

        void Add<EntityType>(EntityType entity);

        void SaveChanges();
    }
}
