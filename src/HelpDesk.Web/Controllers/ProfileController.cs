﻿using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HelpDesk.Web.Controllers
{
    /// <summary>
    /// Control user profile
    /// </summary>
    public class ProfileController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IProfileService _profileService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="profileService"></param>
        public ProfileController(UserManager<User> userManager, IProfileService profileService, IEmailService emailService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <returns>Profile model</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var profile = await _profileService.GetProfileByUserId(user.Id);
            var model = new ProfileViewModel 
            { 
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };            
            return View(model);
        }

        /// <summary>
        /// Change password model
        /// </summary>
        /// <returns>User model</returns>
        public async Task<IActionResult> EditProfile()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var profile = await _profileService.GetProfileByUserId(user.Id);
            var model = new ProfileViewModel { FirstName = profile.FirstName, LastName = profile.LastName, MiddleName = profile.MiddleName };
            return View(model);
        }

        /// <summary>
        /// Edit user profile
        /// </summary>
        /// <param name="editProfile"></param>
        /// <returns>Result edit profile</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel editProfile)
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            var getProfile = await _profileService.GetProfileByUserId(user.Id);

            var profile = new ProfileDto
            {
                Id = getProfile.Id,
                FirstName = editProfile.FirstName,
                LastName = editProfile.LastName,
                MiddleName = editProfile.MiddleName,
            };

            await _profileService.EditProfile(profile);
            return RedirectToAction("Profile");
        }

        /// <summary>
        /// Model for change phone number
        /// </summary>
        /// <returns>View model for change phone number</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangePhoneNumber()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            var model = new ProfileViewModel { Phone = user.PhoneNumber };
            return View(model);
        }

        /// <summary>
        /// Change Phone number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Rezult change phone number</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePhoneNumber(ProfileViewModel number)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.PhoneNumber = number.Phone;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }

        /// <summary>
        /// Model for change Email
        /// </summary>
        /// <returns>View model for change Email</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangeEmail()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);

            var model = new ProfileViewModel { Email = user.Email };
            return View(model);
        }

        /// <summary>
        /// Change Email
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Generate tocken and send email message</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ProfileViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }
    }
}
