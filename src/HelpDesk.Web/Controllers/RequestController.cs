using HelpDesk.BLL.Interfaces;
using HelpDesk.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HelpDesk.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IStatusService _statusService;

        public RequestController(IStatusService statusService)
        {
            _statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
        }

        /// <summary>
        /// View whith properties links.
        /// </summary>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult Properties()
        {
            return View();
        }

    }
}
