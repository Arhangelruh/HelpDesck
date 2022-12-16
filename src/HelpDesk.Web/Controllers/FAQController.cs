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
        /// Get FAQTopic List
        /// </summary>
        /// <returns>list</returns>
        [HttpGet]
        public async Task<IActionResult> FAQTopics()
        {
            var topics = new List<FAQTopicViewModel>();

            var topicList = await _faqService.GetAllFAQTopicAsync();
            if (topicList.Any())
            {
                foreach (var topic in topicList)
                {
                    topics.Add(new FAQTopicViewModel
                    {                        
                        Id = topic.Id,
                        Topic = topic.Topic
                    });
                }
            }
            return View(topics);
        }

        /// <summary>
        /// Model for create FAQ Topic.
        /// </summary>
        /// <returns>View model for create FAQ Topic</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public IActionResult AddFAQTopic()
        {
            return View();
        }

        /// <summary>
        /// Create FAQ Topic.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>FAQ Topics View</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFAQTopic(FAQTopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                var faq = new FAQTopicDto
                {
                    Topic = model.Topic
                };

                await _faqService.AddFaqTopicAsync(faq);

                return RedirectToAction("FAQTopics");
            }
            return View(model);
        }

        /// <summary>
        /// Model for edit FAQ Topic.
        /// </summary>
        /// <returns>View model for edit FAQ Topic</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditFAQTopic(int faqId)
        {
            var getfaqTopicModel = await _faqService.GetFAQTopicByIdAsync(faqId);
            if (getfaqTopicModel == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var model = new FAQTopicViewModel
                {
                    Id = getfaqTopicModel.Id,
                    Topic = getfaqTopicModel.Topic
                };
                return View(model);
            }
        }

        /// <summary>
        /// Edit FAQ Topic.
        /// </summary>
        /// <param name="editFAQ">FAQ Topic model</param>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFAQTopic(FAQTopicViewModel editFAQTopic)
        {
            if (ModelState.IsValid)
            {
                var model = new FAQTopicDto
                {
                    Id = editFAQTopic.Id,
                    Topic = editFAQTopic.Topic
                };

                await _faqService.EditFAQTopicAsync(model);
                return RedirectToAction("FAQTopics", "FAQ");
            }
            else
            {
                return View(editFAQTopic);
            }
        }

        /// <summary>
        /// Delete FAQ Topic.
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteFAQTopic(int id)
        {
            var getfaqTopicModel = await _faqService.GetFAQTopicByIdAsync(id);
            if (getfaqTopicModel == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var result  = await _faqService.DeleteFAQTopicAsync(getfaqTopicModel);
                if (result)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }                
            }
        }

        /// <summary>
        /// Get FAQ List
        /// </summary>
        /// <returns>list</returns>
        [HttpGet]
        public async Task<IActionResult> FAQ(int topicId)
        {
            var modelsfaq = new List<FAQViewModel>();

            var faqTopic = await _faqService.GetFAQTopicByIdAsync(topicId);
            if (faqTopic == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }

            var faqList = await _faqService.GetFAQByTopicAsync(faqTopic.Id);
            if (faqList.Any())
            {
                foreach (var faq in faqList)
                {
                    modelsfaq.Add(new FAQViewModel
                    {
                        Id = faq.Id,
                        FAQTheme = faq.Theme,
                        FAQAnswer = faq.Description,
                        FAQTopic = faq.FAQTopicId
                    });
                }
            }
            ViewBag.NameTopic = faqTopic.Topic;
            ViewBag.IdTopic = faqTopic.Id;
            return View(modelsfaq);
        }

        /// <summary>
        /// Model for create FAQ.
        /// </summary>
        /// <returns>View model for create FAQ</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public IActionResult AddFAQ(int topicId)
        {
            var getTopic = _faqService.GetFAQTopicByIdAsync(topicId);
            if (getTopic == null)
            {
                ViewBag.ErrorMessage = "Тема не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var faq = new FAQViewModel
                {
                    FAQTopic = topicId
                };
                return View(faq);
            }          
        }

        /// <summary>
        /// Create FAQ.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>View FAQ Topics</returns>
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
                    Description = model.FAQAnswer,
                    FAQTopicId = model.FAQTopic
                };

                await _faqService.AddFAQAsync(faq);

                int topicId = model.FAQTopic;

                return RedirectToAction("FAQ", new { topicId });
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
                    FAQAnswer = getfaqModel.Description,
                    FAQTopic = getfaqModel.FAQTopicId
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
                int topicId = editFAQ.FAQTopic;
                await _faqService.EditFAQAsync(model);
                return RedirectToAction("FAQ", new { topicId });
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
        [Authorize(Roles = UserConstants.AdminRole)]
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
                int topicId = getfaqModel.FAQTopicId;
                await _faqService.DeleteFAQAsync(faqId);
                return RedirectToAction("FAQ", new { topicId });
            }
        }
    }
}
