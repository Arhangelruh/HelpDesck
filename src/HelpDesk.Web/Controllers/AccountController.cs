using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Сonstructor
        /// </summary>
        /// <param name="userManager"></param>

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
