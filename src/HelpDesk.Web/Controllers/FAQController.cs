using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
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
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;
        public FAQController(IFAQService faqService)
        {
            _faqService = faqService ?? throw new ArgumentNullException(nameof(faqService));
        }

        /// <summary>
        /// Get FAQ List
        /// </summary>
        /// <returns>list</returns>
        [HttpGet]
        public async Task<IActionResult> FAQ()
        {
            var modelsfaq = new List<FAQViewModel>();

            var faqList = await _faqService.GetAllFAQAsync();
            if (faqList.Any())
            {
                foreach (var faq in faqList)
                {
                    modelsfaq.Add(new FAQViewModel
                    {
                        Id = faq.Id,
                        FAQTheme = faq.Theme,
                        FAQAnswer = faq.Description
                    });
                }
            }
            return View(modelsfaq);
        }

        /// <summary>
        /// Model for create FAQ.
        /// </summary>
        /// <returns>View model for create FAQ</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public IActionResult AddFAQ()
        {
            return View();
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Requests view</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFAQ(FAQViewModel model)
        {
            if (ModelState.IsValid)
            {
                var faq = new FAQDto
                {
                    Theme = model.FAQTheme,
                    Description = model.FAQAnswer
                };

                await _faqService.AddFAQAsync(faq);

                return RedirectToAction("FAQ");
            }
            return View(model);
        }

        /// <summary>
        /// Model for edit FAQ.
        /// </summary>
        /// <returns>View model for edit FAQ</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditFAQ(int faqId)
        {
            var getfaqModel = await _faqService.GetFAQByIdAsync(faqId);
            if (getfaqModel == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var model = new FAQViewModel
                {
                    Id = getfaqModel.Id,
                    FAQTheme = getfaqModel.Theme,
                    FAQAnswer = getfaqModel.Description
                };
                return View(model);
            }
        }

        /// <summary>
        /// Edit FAQ
        /// </summary>
        /// <param name="editFAQ">FAQ model</param>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFAQ(FAQViewModel editFAQ)
        {
            if (ModelState.IsValid)
            {
                var model = new FAQDto
                {
                    Id = editFAQ.Id,
                    Theme = editFAQ.FAQTheme,
                    Description = editFAQ.FAQAnswer
                };

                await _faqService.EditFAQAsync(model);
                return RedirectToAction("FAQ", "FAQ");
            }
            else
            {
                return View(editFAQ);
            }
        }

        /// <summary>
        /// Delete FAQ.
        /// </summary>
        /// <returns>List FAQ</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteFAQ(int faqId)
        {
            var getfaqModel = await _faqService.GetFAQByIdAsync(faqId);
            if (getfaqModel == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                await _faqService.DeleteFAQAsync(faqId);
                return RedirectToAction("FAQ");
            }
        }
    }
}
