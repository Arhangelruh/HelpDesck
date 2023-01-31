using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.Web.Services;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileController> _logger;

        private readonly string _fileDirectory;

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        private readonly string[] _permittedExtensions = { ".doc", ".docx", ".zip", ".jpg", ".jpeg", ".png" };
        private readonly string _targetInstructionPath;


        public FileController(IFileService fileService, IWebHostEnvironment environment, ILogger<FileController> logger, IConfiguration config)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _targetInstructionPath = config.GetValue<string>("StoredInstructionPath");
            _fileDirectory = Path.Combine(this._environment.WebRootPath, _targetInstructionPath);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFile(SaveFileViewModel saveFile, int Id)
        {
            if (!ModelState.IsValid)
            {
                if (Id == 0)
                {
                    ViewBag.ErrorTitle = "Ошибка";
                    ViewBag.ErrorMessage = "Ошибка загрузки, проверьте что файл соответствует размерам";
                    return View("~/Views/Error/Error.cshtml");
                }
                else
                {
                    var requestId = Id;
                    return RedirectToAction("GetRequest", "Request", new { requestId });
                }
            }
            if (saveFile.FileBody.FileName.Length > ConfigurationContants.SqlMaxLengthMedium)
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = $"Имя файла не должно превышать {ConfigurationContants.SqlMaxLengthMedium} символов.";
                return View("~/Views/Error/Error.cshtml");
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
            ViewBag.ErrorTitle = "Ошибка";
            ViewBag.ErrorMessage = "Ошибка, файл не найден";
            return View("~/Views/Error/Error.cshtml");
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


        /// <summary>
        /// Get files instructions from folder.
        /// </summary>
        /// <returns></returns>
        public IActionResult Instructions()
        {
            //var fileDirectory = Path.Combine(this._environment.WebRootPath,
            //"instructions");

            List<InstructionViewModel> fileModels = new();

            foreach (var item in Directory.GetFiles(_fileDirectory))
            {

                fileModels.Add(new InstructionViewModel()
                {
                    FileName = Path.GetFileName(item),
                    FilePath = "/" + _targetInstructionPath + $"/{Path.GetFileName(item)}"
                });
            }

            return View(fileModels);
        }

        [HttpGet]
        public IActionResult AddFilePhysical()
        {
            return View();
        }

        /// <summary>
        /// Save  file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Requests view</returns>
        // [Authorize]
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = UploadFileConstant.UploadMiddleValue)]
        public async Task<IActionResult> AddFilePhysical(UploadInstructionViewModel instruction)
        {
            if (ModelState.IsValid)
            {
                var streamedFileContent = await FileHelpers.ChangeFormFile(instruction.FileBody,
                     ModelState, _permittedExtensions, UploadFileConstant.UploadMiddleValue);
            }


            if (!ModelState.IsValid)
            {                
                return BadRequest(ModelState);
            }

            if (instruction.FileBody.Length > 0)
            {


                string fileName = Path.GetFileName(instruction.FileBody.FileName);

                using var stream = System.IO.File.Create(_fileDirectory + "/" + fileName);
                await instruction.FileBody.CopyToAsync(stream);
            }

            return RedirectToAction("Instructions");
        }
    }
}
