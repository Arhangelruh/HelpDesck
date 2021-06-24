using HelpDesk.BLL.Interfaces;
using HelpDesk.Common.Constants;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestsService _requestsService;

        public RequestController(IRequestsService requestsService)
        {
            _requestsService = requestsService ?? throw new ArgumentNullException(nameof(requestsService));
        }

        /// <summary>
        /// View whith properties links
        /// </summary>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult Properties()
        {
            return View();
        }

        /// <summary>
        /// Get statuses
        /// </summary>
        /// <returns>List status model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Statuses()
        {
            var statuses = await _requestsService.GetStatusesAsync();
            var models = new List<StatusViewModel>();

            if (statuses.Any())
            {
                foreach (var status in statuses)
                {                    
                    models.Add(
                    new StatusViewModel
                    {
                        Id = status.Id,
                        StatusName = status.StatusName,
                        Queue = status.Queue,
                        Access = status.Access
                    });
                }
                return View(models);
            }
            else
            {
                return Content("Статусы не найдены");
            }           
        }
    }
}
