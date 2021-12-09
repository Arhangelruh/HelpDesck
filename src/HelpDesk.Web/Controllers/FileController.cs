using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        /// <summary>
        /// Save file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Requests view</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFile(SaveFileViewModel saveFile, int Id)
        {
          //  var name = saveFile.FileBody.FileName;
            if (saveFile.FileBody != null)
            {
                var binaryReader = new BinaryReader(saveFile.FileBody.OpenReadStream());
                byte[] fileData = binaryReader.ReadBytes((int)saveFile.FileBody.Length);

                var fileDto = new FileDto {
                  Name = saveFile.FileBody.FileName,
                  ContentType = saveFile.FileBody.ContentType,
                  ProblemId = Id,
                  FileBody = fileData
                };

                await _fileService.AddFileAsync(fileDto);
                var requestId = Id;
                return RedirectToAction("GetRequest", "Request", new { requestId });
            }
            return Content("Ошибка, файл не найден");
        }

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<FileResult> GetFile(int fileId)
        {
            var file =await _fileService.GetFileAsync(fileId);
                    
            return File(file.FileBody, file.ContentType,file.Name);        
        }
    }
}
