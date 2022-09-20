using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using HelpDesk.Web.Services;
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
        private readonly ICommentService _commentService;
        private readonly IFileService _fileService;


        public RequestController(
            UserManager<User> userManager,
            IStatusService statusService,
            IRequestsService requestsService,
            IProfileService profileService,
            ICommentService commentService,
            IFileService fileService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
            _requestsService = requestsService ?? throw new ArgumentNullException(nameof(requestsService));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
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
        public async Task<IActionResult> Requests(string sortOrder, int? page, int? pagesize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NumberSortParm"] = String.IsNullOrEmpty(sortOrder) ? "number_ask" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewData["CreatorSortParm"] = sortOrder == "Creator" ? "Creator_desc" : "Creator";
            ViewData["ExecuterSortParm"] = sortOrder == "Executer" ? "Executer_desc" : "Executer";

            int pageSize = (int)(pagesize == null ? 20 : pagesize);
            ViewData["PageSize"] = pageSize;

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
                        if (request.StatusId != 1)
                        {
                            var status = statuses.FirstOrDefault(status => status.Id == request.StatusId);
                            var profile = profiles.FirstOrDefault(profile => profile.Id == request.ProfileCreatorId);
                            var profileAdmin = profiles.FirstOrDefault(profile => profile.Id == request.ProfileAdminId);
                            string profileName, administratorName;

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
                                IncomingDate = request.IncomingDate,
                                Admin = administratorName
                            });
                        }
                    }
                }

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
                            IncomingDate = request.IncomingDate,
                            Admin = administratorName
                        });
                    }
                }
            }

            switch (sortOrder)
            {
                case "number_ask":
                    modelsRequests = modelsRequests.OrderBy(n => n.Id).ToList();
                    break;
                case "Date":
                    modelsRequests = modelsRequests.OrderBy(d => d.IncomingDate).ToList();
                    break;
                case "Date_desc":
                    modelsRequests = modelsRequests.OrderByDescending(d => d.IncomingDate).ToList();
                    break;
                case "Status":
                    modelsRequests = modelsRequests.OrderBy(s => s.Status).ToList();
                    break;
                case "Status_desc":
                    modelsRequests = modelsRequests.OrderByDescending(s => s.Status).ToList();
                    break;
                case "Creator":
                    modelsRequests = modelsRequests.OrderBy(c => c.Creator).ToList();
                    break;
                case "Creator_desc":
                    modelsRequests = modelsRequests.OrderByDescending(c => c.Creator).ToList();
                    break;
                case "Executer":
                    modelsRequests = modelsRequests.OrderBy(e => e.Admin).ToList();
                    break;
                case "Executer_desc":
                    modelsRequests = modelsRequests.OrderByDescending(e => e.Admin).ToList();
                    break;
                default:
                    modelsRequests = modelsRequests.OrderByDescending(n => n.Id).ToList();
                    break;
            }
            return View(PaginatedList<RequestViewModel>.Create(modelsRequests, page ?? 1, pageSize));
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

                var request = new RequestDto
                {
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

        /// <summary>
        /// get request model.
        /// </summary>
        /// <returns>View model request info</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRequest(int requestId)
        {
            var username = User.Identity.Name;
            var loginUser = await _userManager.FindByNameAsync(username);

            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);
            if (getRequestModel is null)
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Заявка не найдена!";
                return View("~/Views/Error/Error.cshtml");
            }
            string userName, adminName;

            var userProfile = await _profileService.GetProfileByIdAsync(getRequestModel.ProfileCreatorId);
            var adminProfile = await _profileService.GetProfileByIdAsync(getRequestModel.ProfileAdminId);
            if (userProfile is null)
            {
                userName = "Not Found";
            }
            else
            {
                if (userProfile.LastName != null || userProfile.FirstName != null)
                {
                    userName = userProfile.LastName + " " + userProfile.FirstName;
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(userProfile.UserId);
                    userName = user.UserName;
                }
            }

            if (adminProfile is null)
            {
                adminName = "не назначен";
            }
            else
            {
                if (adminProfile.LastName != null || adminProfile.FirstName != null)
                {
                    adminName = adminProfile.LastName + " " + adminProfile.FirstName;
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(adminProfile.UserId);
                    adminName = user.UserName;
                }
            }

            var status = await _statusService.GetStatusByIdAsync(getRequestModel.StatusId);

            var commentsDto = await _commentService.GetCommentsByRequestAsync(requestId);

            var statuses = new List<StatusDto>();
            var checkRole = await _userManager.IsInRoleAsync(loginUser, UserConstants.AdminRole);
            if (checkRole)
            {
                var getStatuses = await _statusService.GetStatusesAsync();
                foreach (var getstatus in getStatuses)
                {
                    if (getstatus.Queue > 3)
                    {
                        statuses.Add(getstatus);
                    }
                }
            }
            else
            {
                if (status.Access == true && status.Queue > 3)
                {
                    var getStatuses = await _statusService.GetStatusesAsync();

                    foreach (var getstatus in getStatuses.OrderBy(sort => sort.Queue))
                    {
                        if (getstatus.Access == true && getstatus.Queue > status.Queue)
                        {
                            statuses.Add(getstatus);
                            break;
                        }
                    }
                }
            }

            var comments = new List<CommentViewModel>();

            foreach (var commentDto in commentsDto)
            {
                string commentCreator;
                var commentCreatorProfile = await _profileService.GetProfileByIdAsync(commentDto.ProfileId);
                if (commentCreatorProfile is null)
                {
                    commentCreator = "не найден";
                }
                else
                {
                    if (commentCreatorProfile.LastName != null || commentCreatorProfile.FirstName != null)
                    {
                        commentCreator = commentCreatorProfile.LastName + " " + commentCreatorProfile.FirstName;
                    }
                    else
                    {
                        var user = await _userManager.FindByIdAsync(commentCreatorProfile.UserId);
                        commentCreator = user.UserName;
                    }
                }

                comments.Add(new CommentViewModel
                {
                    Id = commentDto.Id,
                    Profile = commentCreator,
                    CreateComment = commentDto.CreateComment.ToString("dd.MM.yyyy H:mm:ss"),
                    Comment = commentDto.Comment
                }); ;
            }

            var files = await _fileService.GetFilesNamesAsync(requestId);

            var requestViewModel = new FullRequestViewModel
            {
                Id = getRequestModel.Id,
                Theme = getRequestModel.Theme,
                Description = getRequestModel.Description,
                Ip = getRequestModel.Ip,
                Creator = userName,
                Status = status.StatusName,
                StatusQueue = status.Queue,
                IncomingDate = getRequestModel.IncomingDate,
                Admin = adminName,
                Comments = comments,
                Statuses = statuses,
                Files = files
            };

            return View(requestViewModel);
        }

        /// <summary>
        /// delete request.
        /// </summary>
        /// <returns>View requests</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);

            var status = await _statusService.GetStatusByIdAsync(getRequestModel.StatusId);

            if (status.Queue == 1)
            {
                await _commentService.DeleteCommentsAsync(getRequestModel.Id);
                await _requestsService.DeleteRequestAsync(getRequestModel);
                return RedirectToAction("Requests");
            }
            else
            {
                ViewBag.ErrorMessage = "Нельзя удалить заявки отправленные в работу";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Model for edit raquest.
        /// </summary>
        /// <returns>View model for edit request if status request = firststatus </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditRequest(int requestId)
        {
            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);
            var status = await _statusService.GetStatusByIdAsync(getRequestModel.StatusId);
            if (status.Queue == 1)
            {
                var requestViewModel = new RequestViewModel
                {
                    Id = getRequestModel.Id,
                    Theme = getRequestModel.Theme,
                    Description = getRequestModel.Description,
                    Ip = getRequestModel.Ip
                };
                return View(requestViewModel);
            }
            else
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Редактировать заявки отправленные в работу запрещено.";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Edit problem
        /// </summary>
        /// <param name="editRequest"></param>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequest(RequestViewModel editRequest)
        {
            if (ModelState.IsValid)
            {
                var problem = new RequestDto
                {
                    Id = editRequest.Id,
                    Theme = editRequest.Theme,
                    Description = editRequest.Description,
                    Ip = editRequest.Ip
                };

                await _requestsService.EditRequestAsync(problem);
                return RedirectToAction("GetRequest", "Request", new { requestId = editRequest.Id });
            }
            else
            {
                return View(editRequest);
            }
        }

        /// <summary>
        /// Send problem
        /// </summary>
        /// <param name="requestId"></param>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SendRequest(int requestId)
        {
            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);
            var status = await _statusService.SearchStatusAsync(2);

            await _requestsService.ChangeStatusAsync(getRequestModel, status.Id);

            return RedirectToAction("GetRequest", "Request", new { requestId });
        }

        /// <summary>
        /// Take problem to work
        /// </summary>
        /// <param name="requestId"></param>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> InWork(int requestId)
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var profile = await _profileService.GetProfileByUserId(user.Id);

            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);
            var status = await _statusService.SearchStatusAsync(3);

            await _requestsService.AddToWorkAsync(getRequestModel, profile);
            await _requestsService.ChangeStatusAsync(getRequestModel, status.Id);

            return RedirectToAction("GetRequest", "Request", new { requestId });
        }

        /// <summary>
        /// Change status
        /// </summary>
        /// <param name="requestId"></param>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(int requestId, int statusId)
        {
            var getRequestModel = await _requestsService.GetRequestByIdAsync(requestId);
            if (getRequestModel is null)
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Заявка не найдена.";
                return View("~/Views/Error/Error.cshtml");
            }

            var status = await _statusService.GetStatusByIdAsync(statusId);
            if (status is null)
            {
                ViewBag.ErrorTitle = "Ошибка";
                ViewBag.ErrorMessage = "Статус не найден.";
                return View("~/Views/Error/Error.cshtml");
            }

            await _requestsService.ChangeStatusAsync(getRequestModel, status.Id);

            return RedirectToAction("GetRequest", "Request", new { requestId });
        }

        /// <summary>
        /// Create comment.
        /// </summary>
        /// <param name="comment">comment text</param>
        /// <param name="id">id request</param>
        /// <returns>Requests view</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(string comment, int Id)
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var profile = await _profileService.GetProfileByUserId(user.Id);

            if (comment != null)
            {
                var newComment = new CommentDto
                {
                    ProblemId = Id,
                    ProfileId = profile.Id,
                    Comment = comment
                };
                await _commentService.AddCommentAsync(newComment);
                var requestId = Id;
                return RedirectToAction("GetRequest", "Request", new { requestId });
            }
            ViewBag.ErrorTitle = "Ошибка";
            ViewBag.ErrorMessage = "Не найден текст комментария.";
            return View("~/Views/Error/Error.cshtml");
        }
    }
}
