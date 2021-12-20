using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
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
        [DisableRequestSizeLimit,
    RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> AddFile(SaveFileViewModel saveFile, int Id)
        {
            if (!ModelState.IsValid)
            {
                if (Id == 0)
                {
                    return Content("Ошибка загрузки, проверьте что файл соответствует размерам");
                }
                else
                {
                    var requestId = Id;
                    return RedirectToAction("GetRequest", "Request", new { requestId });
                }
            }
            if (saveFile.FileBody.FileName.Length > ConfigurationContants.SqlMaxLengthMedium)
            {
                return Content($"Имя файла не должно превышать {ConfigurationContants.SqlMaxLengthMedium} символов.");
            }

            if (saveFile.FileBody != null)
            {
                var binaryReader = new BinaryReader(saveFile.FileBody.OpenReadStream());
                byte[] fileData = binaryReader.ReadBytes((int)saveFile.FileBody.Length);

                var fileDto = new FileDto
                {
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
        [HttpGet]
        public async Task<FileResult> GetFile(int fileId)
        {
            var file = await _fileService.GetFileAsync(fileId);

            return File(file.FileBody, file.ContentType, file.Name);
        }

        /// <summary>
        /// Get file.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<ActionResult> DeleteFile(int fileId, int requestId)
        {
            await _fileService.DeleteFileAsync(fileId);
            return RedirectToAction("GetRequest", "Request", new { requestId });
        }
    }
}
