using Hangfire.Dashboard;
using HelpDesk.Common.Constants;

namespace HelpDesk.BLL.Services
{
    /// <summary>
    /// Check user adin role from Hangfire Dashbord. 
    /// </summary>
    public class MyHangfireDashbordAutorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Check autorise user.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true if user admin or false if no</returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext.User.IsInRole(UserConstants.AdminRole))
            {
                return true;
            }
            return false;
        }
    }
}

