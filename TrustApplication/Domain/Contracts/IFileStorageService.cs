using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustApplication.Models;

namespace TrustApplication.Domain.Contracts
{
    public interface IFileStorageService
    {
        Task<List<FileEntity>> UploadFilesToStorage(IEnumerable<FileEntity> files);
    }
}
