using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly IStatusService _statusService;
        private readonly IRequestsService _requestsService;
        private readonly UserManager<User> _userManager;
        private readonly IProfileService _profileService;

        public RequestController(UserManager<User> userManager, IStatusService statusService, IRequestsService requestsService, IProfileService profileService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
            _requestsService = requestsService ?? throw new ArgumentNullException(nameof(requestsService));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        }

        /// <summary>
        /// View whith properties links.
        /// </summary>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult Properties()
        {
            return View();
        }

        /// <summary>
        /// Get requests.
        /// </summary>
        /// <returns>List requests</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var ifAdmin = await _userManager.IsInRoleAsync(user, UserConstants.AdminRole);
            var modelsRequests = new List<RequestViewModel>();

            var statuses = await _statusService.GetStatusesAsync();
            var profiles = await _profileService.GetAsyncProfiles();            

            if (ifAdmin)
            {
                var getRequests = await _requestsService.GetAllRequestsAsync();
                if (getRequests.Any())
                {
                    foreach (var request in getRequests)
                    {
                        var status = statuses.FirstOrDefault(status => status.Id == request.StatusId);
                        var profile = profiles.FirstOrDefault(profile => profile.Id == request.ProfileCreatorId);
                        var profileAdmin = profiles.FirstOrDefault(profile => profile.Id == request.ProfileAdminId);
                        string profileName, administratorName;

                        var requestCreate = request.IncomingDate.ToString("dd.MM.yyyy H:mm:ss");

                        if (profile is null)
                        {
                            profileName = "Not found";
                        }
                        else
                        {
                            profileName = profile.LastName + " " + profile.FirstName;
                            if (profile.LastName is null && profile.FirstName is null)
                            {
                                var getCreator = await _userManager.FindByIdAsync(profile.UserId);
                                profileName = getCreator.UserName;
                            }
                        }

                        if(profileAdmin is null)
                        {
                            administratorName = "Не в работе";
                        }
                        else
                        {
                            administratorName = profileAdmin.LastName + " " + profileAdmin.FirstName;
                            if(profileAdmin.LastName is null && profileAdmin.FirstName is null)
                            {
                                var getAdmin = await _userManager.FindByIdAsync(profileAdmin.UserId);
                                administratorName = getAdmin.UserName;
                            }
                        }

                        modelsRequests.Add(new RequestViewModel
                        {
                            Id = request.Id,
                            Theme = request.Theme,
                            Description = request.Description,
                            Ip = request.Ip,
                            Creator = profileName,
                            Status = status.StatusName,
                            IncomingDate = requestCreate,
                            Admin = administratorName
                        });
                    }
                }
                return View(modelsRequests);
            }
            else
            {
                var profile = await _profileService.GetProfileByUserId(user.Id);
                var getRequests = await _requestsService.GetRequestsByUserAsync(profile.Id);

                if (getRequests.Any())
                {
                    foreach (var request in getRequests)
                    {
                        var status = statuses.FirstOrDefault(status => status.Id == request.StatusId);
                        var requestCreate = request.IncomingDate.ToString("dd.MM.yyyy H:mm:ss");
                        var profileAdmin = profiles.FirstOrDefault(profile => profile.Id == request.ProfileAdminId);

                        string profileName, administratorName;
                        if (profile is null)
                        {
                            profileName = "Not found";
                        }
                        else
                        {
                            profileName = profile.LastName + " " + profile.FirstName;
                        }

                        if (profileAdmin is null)
                        {
                            administratorName = "Не в работе";
                        }
                        else
                        {
                            administratorName = profileAdmin.LastName + " " + profileAdmin.FirstName;
                            if (profileAdmin.LastName is null && profileAdmin.FirstName is null)
                            {
                                var getAdmin = await _userManager.FindByIdAsync(profileAdmin.UserId);
                                administratorName = getAdmin.UserName;
                            }
                        }

                        modelsRequests.Add(new RequestViewModel
                        {
                            Id = request.Id,
                            Theme = request.Theme,
                            Description = request.Description,
                            Ip = request.Ip,
                            Creator = profileName,
                            Status = status.StatusName,
                            IncomingDate = requestCreate,
                            Admin = administratorName
                        });
                    }
                }
                return View(modelsRequests);
            }
        }

        /// <summary>
        /// Model for create raquest.
        /// </summary>
        /// <returns>View model for create request</returns>
        [Authorize(Roles = UserConstants.UserRole)]
        [HttpGet]
        public IActionResult AddRequest()
        {            
            return View();
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Requests view</returns>
        [Authorize(Roles = UserConstants.UserRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequest(RequestViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var profile = await _profileService.GetProfileByUserId(user.Id);

            if (ModelState.IsValid)
            {

                var request = new RequestDto {
                Theme = model.Theme,
                Ip = model.Ip,
                Description = model.Description,
                ProfileCreatorId = profile.Id
                };

                await _requestsService.AddRequestAsync(request);

                return RedirectToAction("Requests");
            }
            return View(model);
        }
    }
}
