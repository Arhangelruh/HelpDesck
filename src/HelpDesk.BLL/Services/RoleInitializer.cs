using HelpDesk.BLL.Interfaces;
using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <summary>
    /// Create user and admin role and first administrator.
    /// </summary>
    public class RoleInitializer
    {
        /// <summary>
        /// Create user and admin role and first administrator.
        /// </summary>
        /// <returns>result</returns>
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string userName = UserConstants.FirstAdmin;
            

            if (await roleManager.FindByNameAsync(UserConstants.AdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserConstants.AdminRole));
            }
            if (await roleManager.FindByNameAsync(UserConstants.UserRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserConstants.UserRole));
            }
            if (await userManager.FindByNameAsync(userName) == null)
            {
                User admin = new User { UserName = userName };
                IdentityResult result = await userManager.CreateAsync(admin, UserConstants.FirstPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UserConstants.AdminRole);
                }
            }
        }
    }
}
