using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
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

        /// <summary>
        /// Get statuses.
        /// </summary>
        /// <returns>List status model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Statuses()
        {
            var statuses = await _statusService.GetStatusesAsync();
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

        /// <summary>
        /// Model for create status.
        /// </summary>
        /// <returns>View model for create status</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public IActionResult CreateStatus()
        {
            List<bool> acces = new() { true, false };

            ViewBag.acces = new SelectList(acces);
            return View();
        }

        /// <summary>
        /// Create status.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Statuses view</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStatus(StatusViewModel model)
        {
            List<bool> acces = new() { true, false };

            ViewBag.acces = new SelectList(acces);

            if (ModelState.IsValid)
            {
                var statuses = await _statusService.GetStatusesAsync();
                var checkStatus = statuses.FirstOrDefault(queue => queue.Queue == model.Queue);

                if (checkStatus == null)
                {

                    var status = new StatusDto()
                    {
                        StatusName = model.StatusName,
                        Queue = model.Queue,
                        Access = model.Access

                    };

                    await _statusService.AddStatusAsync(status);

                    return RedirectToAction("Statuses");
                }
                else
                {
                    ModelState.AddModelError("Error", "Статус с такой очередностью уже существует");
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Model for edit status.
        /// </summary>
        /// <returns>View model for edit status</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditStatus(int id)
        {
            List<bool> acces = new() { true, false };
            ViewBag.acces = new SelectList(acces);

            var getStatusDto = await _statusService.GetStatusByIdAsync(id);

            var model = new StatusViewModel
            {
                Id = getStatusDto.Id,
                StatusName = getStatusDto.StatusName,
                Queue = getStatusDto.Queue,
                Access = getStatusDto.Access
            };

            return View(model);
        }

        /// <summary>
        /// Edit status.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Statuses view</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(StatusViewModel model)
        {
            List<bool> acces = new() { true, false };
            ViewBag.acces = new SelectList(acces);

            if (ModelState.IsValid)
            {
                var getStatus = await _statusService.GetStatusByIdAsync(model.Id);

                if (getStatus.Queue != 1 && getStatus.Queue != 2)
                {

                    if (model.Queue != 1 && model.Queue != 2)
                    {
                        var statuses = await _statusService.GetStatusesAsync();
                        var checkStatus = statuses.FirstOrDefault(queue => queue.Queue == model.Queue);

                        if (checkStatus == null || checkStatus.Id == getStatus.Id)
                        {
                            var status = new StatusDto()
                            {
                                Id = model.Id,
                                StatusName = model.StatusName,
                                Queue = model.Queue,
                                Access = model.Access
                            };

                            await _statusService.EditStatusAsync(status);

                            return RedirectToAction("Statuses");
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Статус с такой очередностью уже существует");
                            return View(model);
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("Error", "Статус с такой очередностью уже существует");
                        return View(model);

                    }
                }
                else
                {
                    if (getStatus.Queue == model.Queue && getStatus.Access == model.Access)
                    {
                        var status = new StatusDto()
                        {
                            Id = model.Id,
                            StatusName = model.StatusName,
                            Queue = model.Queue,
                            Access = model.Access
                        };

                        await _statusService.EditStatusAsync(status);

                        return RedirectToAction("Statuses");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Менять очередность и доступность на статусах первой и второй очередности нельзя.");
                        return View(model);
                    }
                }                
            }
            return View(model);
        }

        /// <summary>
        /// Model for delete status.
        /// </summary>
        /// <returns>View model for delete status</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var getStatusDto = await _statusService.GetStatusByIdAsync(id);

            if (getStatusDto.Queue != 1 && getStatusDto.Queue != 2)
            {
                var result = await _statusService.DeleteStatusAsync(getStatusDto);
                if (result)
                {
                    //return RedirectToAction("Statuses");
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
            else
            {
                return Json("error");
            }          
        }
    }
}
