using HelpDesk.Web.Attributs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using HelpDesk.Common.Constants;

namespace HelpDesk.Web.ViewModels
{
	public class UploadInstructionViewModel
	{

			/// <summary>
			/// File Name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Body file.
			/// </summary>
			[Required]
			[MaxFileSize(UploadFileConstant.UploadMiddleValue, ErrorMessage = "Это уж слишком")]
			public IFormFile FileBody { get; set; }		
	}
}

