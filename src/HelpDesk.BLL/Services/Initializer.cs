using HelpDesk.BLL.Interfaces;
using HelpDesk.BLL.Models;
using HelpDesk.Common.Constants;
using HelpDesk.Common.Interfaces;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Services
{
    /// <summary>
    /// Create user and admin role and first administrator.
    /// </summary>
    public class Initializer
    {

        /// <summary>
        /// Create user and admin role and first administrator.
        /// </summary>
        /// <returns>result</returns>
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRepository<Profile> repository, IRequestsService requestsService)
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
                    await repository.AddAsync(new Profile { UserId = admin.Id });
                    await repository.SaveChangesAsync();
                }
            }

            if(await requestsService.SearchStatusAsync(1) == null)
            {
                await requestsService.AddStatusAsync(new StatusDto { StatusName = StatusConstant.FirstStatus, Queue = 1, Access = true });
            }

            if (await requestsService.SearchStatusAsync(2) == null)
            {
                await requestsService.AddStatusAsync(new StatusDto { StatusName = StatusConstant.SecondStatus, Queue = 2, Access = true });
            }
        }
    }
}
