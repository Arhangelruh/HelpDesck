using Hangfire;
using Hangfire.Storage;
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
    public class SchedulleController : Controller
    {
        private readonly IEventService _eventService;
        public SchedulleController(IEventService eventService)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        /// <summary>
        /// Get jobs
        /// </summary>
        /// <returns>jobs models</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult Schedulle()
        {
            List<ReccuringJobViewModel> models = new();
            List<RecurringJobDto> recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            if (recurringJobs.Any())
            {
                foreach (var job in recurringJobs)
                {
                    models.Add(new ReccuringJobViewModel
                    {
                        Id = job.Id,
                        Cron = job.Cron,
                        Job = job.Job
                    });
                }
            }
            return View(models);
        }

        /// <summary>
        /// Add job.
        /// </summary>
        /// <returns>Job model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult CreateJob()
        {
            return View();
        }

        /// <summary>
        /// Add job.
        /// </summary>
        /// <param name="model">Job view model</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserConstants.AdminRole)]
        public async Task<IActionResult> CreateJob(ReccuringJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _eventService.AddJobScheduller(model.Cron);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Schedulle");
        }

        /// <summary>
        /// Edit job
        /// </summary>
        /// <param name="id">id job</param>
        /// <returns>Job model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        public IActionResult EditJob(string id)
        {
            var job = _eventService.GetJobScheduller(id).GetAwaiter().GetResult();
            var model = new ReccuringJobViewModel
            {
                Id = job.Id,
                Cron = job.Cron,
                Job = job.Job
            };

            return View(model);
        }

        /// <summary>
        /// Edit job.
        /// </summary>
        /// <param name="model">job view model</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserConstants.AdminRole)]
        public async Task<IActionResult> EditJob(ReccuringJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _eventService.EditJobScheduller(model.Id, model.Cron);
            }
            else
            {
                return View(model);
            }
            return RedirectToAction("Schedulle");
        }

        /// <summary>
        /// Delete job.
        /// </summary>
        /// <param name="model">id job</param>
        [HttpGet]
        [Authorize(Roles = UserConstants.AdminRole)]
        public async Task<IActionResult> DeleteJob(string id)
        {
            await _eventService.DeleteJobScheduller(id);
            return RedirectToAction("Schedulle");
        }
    }
}
