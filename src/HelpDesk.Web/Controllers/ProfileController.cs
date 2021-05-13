using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using HelpDesk.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="profileService"></param>
        public ProfileController(UserManager<User> userManager, IProfileService profileService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
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
        [Authorize]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ProfileViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }

        /// <summary>
        /// Get profiles
        /// </summary>
        /// <returns>List profile model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> UserProfiles()
        {
            var profiles = await _profileService.GetAsyncProfiles();
            var models = new List<UserViewModel>();

            foreach (var profile in profiles)
            {
                var user = await _userManager.FindByIdAsync(profile.UserId);
                var ifAdmin = await _userManager.IsInRoleAsync(user, UserConstants.AdminRole);
                string role;
                bool Locking = false;

                if (ifAdmin)
                {
                    role = UserConstants.AdminRole;
                }
                else
                {
                    role = UserConstants.UserRole;
                }

                if (user.LockoutEnd >= DateTime.UtcNow)
                {
                    Locking = true;
                }

                models.Add(
                new UserViewModel
                {
                    Id = profile.Id,
                    Login = user.UserName,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    MiddleName = profile.MiddleName,
                    Email = profile.Email,
                    Phone = profile.MobileNumber,
                    IsAdmin = role,
                    Locking = Locking
                });
            }

            return View(models);
        }

        /// <summary>
        /// Model for create user account
        /// </summary>
        /// <returns>View model for create user</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public IActionResult CreateUser()
        {
            List<string> roles = new List<string>();

            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            roles.Reverse();
            ViewBag.roles = new SelectList(roles);
            return View();
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Confirm email view</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            List<string> roles = new List<string>();

            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            roles.Reverse();
            ViewBag.roles = new SelectList(roles);

            if (ModelState.IsValid)
            {
                var user = new User { Email = model.Email, PhoneNumber = model.Phone, UserName = model.Login };

                var searchUser = await _userManager.FindByNameAsync(model.Login);

                if (searchUser == null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var profile = new ProfileDto()
                        {
                            UserId = user.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName

                        };

                        await _profileService.AddProfileAsync(profile);
                        var addrole = await _userManager.AddToRoleAsync(user, model.IsAdmin);
                        if (addrole.Succeeded)
                        {
                            return RedirectToAction("UserProfiles");
                        }
                        else
                        {

                            return View(model);
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return Content("Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }

        /// <summary>
        /// Lock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>result add to table</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Lock(int Id)
        {
            await _profileService.Locking(Id);
            
            return RedirectToAction("UserProfiles");
        }

        /// <summary>
        /// Unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>result add to table</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> UnLock(int Id)
        {
            await _profileService.UnLock(Id);

            return RedirectToAction("UserProfiles");
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edituser model</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> EditUser(int Id)
        {
            var profile = await _profileService.GetProfileByIdAsync(Id);
            var user = await _userManager.FindByIdAsync(profile.UserId);


            List<string> roles = new List<string>();

            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            roles.Reverse();
            ViewBag.roles = new SelectList(roles);

            var model = new EditUserViewModel
            {
                UserId = user.Id,
                Login = user.UserName,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };
            return View(model);
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="editUser"></param>
        /// <returns>Result edit user account</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            List<string> roles = new List<string>();

            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            roles.Reverse();
            ViewBag.roles = new SelectList(roles);

            if (ModelState.IsValid)
            {
                var searchUser = await _userManager.FindByNameAsync(model.Login);

                if (searchUser != null && searchUser.Id !=model.UserId)
                {
                    return Content("Пользователь с таким логином уже существует");                   
                }
                else
                {
                    if (model.Password != null)
                    {
                        var getUser = await _userManager.FindByIdAsync(model.UserId);
                        if (getUser != null)
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(getUser);
                            var changePassword = await _userManager.ResetPasswordAsync(getUser, token, model.Password);
                            if (!changePassword.Succeeded)
                            {
                                return View(model);
                            }
                        }
                    }

                    var user = new UserDto
                    {
                        Id = model.UserId,
                        Login = model.Login,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        EMail = model.Email,
                        MobileNumber = model.Phone,
                        Role = model.Role
                    };
                    await _profileService.EditUserAsync(user);
                    return RedirectToAction("UserProfiles");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Result delete user account</returns>
        [Authorize(Roles = UserConstants.AdminRole)]
        [HttpGet]
        public async Task<ActionResult> DeleteUser(int id)
        {
           await _profileService.DeleteUserAsync(id);
            return RedirectToAction("UserProfiles");
        }
    }
}
