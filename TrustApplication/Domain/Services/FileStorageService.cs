using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustApplication.Models;
using TrustApplication.Models.Infrastructure;
using TrustApplication.Domain.Contracts;

namespace TrustApplication.Domain.Services
{
    /// <summary>
    /// Service for File Storage related activities
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly IFileStorageRepository _repository;
        public FileStorageService(IFileStorageRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Uploads file meta data and its content to the system file storage
        /// </summary>
        public List<FileEntity> UploadFilesToStorage(IEnumerable<FileEntity> files)
        {
            List<FileEntity> _successfulFiles = new List<FileEntity>();
            foreach (FileEntity file in files)
            {
                if (!ValidateFile(file))
                {
                    continue;
                }
                _repository.Add(file);
                _repository.SaveChanges();
                //file.Content = null;
                _successfulFiles.Add(file);
            }
            return _successfulFiles;
        }

        /// <summary>
        /// Validates whether if the File is valid for storage
        /// </summary>
        private static Boolean ValidateFile(FileEntity file)
        {
            if (file == null) //File is null
            {
                return false;
            }
            else if (file.Filename.Length >= 255) //Windows' limit on filenames
            {
                return false;
            }
            return true;
        }
    }
}
