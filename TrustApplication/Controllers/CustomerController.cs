using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustApplication.Models;
using TrustApplication.Domain.Contracts;

namespace TrustApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase //else Controller for .NET MVC (as Controller inherits ControllerBase)
    {
        private readonly IFileStorageService _fileStorageService;

        public CustomerController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost("PostFileUpload")]
        public async Task<IActionResult> PostFileUpload([FromBody] ICollection<FileViewModel> userUploadedFiles)
        {
            try
            {
                if (userUploadedFiles.Count <= 0)
                {
                    throw new("Request does not have file(s) to process.");
                }
                List<FileEntity> _userUploadedFiles = new List<FileEntity>();
                foreach (FileViewModel file in userUploadedFiles)
                {
                    _userUploadedFiles.Add(new FileEntity
                    {
                        Filename = file.Filename,
                        StoredDateTime = file.StoredDateTime,
                        Content = file.Content
                    });
                }

                IList<FileEntity> _successfullyUploadedFiles = await _fileStorageService.UploadFilesToStorage(_userUploadedFiles);

                return Ok(_successfullyUploadedFiles);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
