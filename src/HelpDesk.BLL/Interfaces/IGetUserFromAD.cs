using HelpDesk.BLL.Models;
using System.Collections.Generic;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Service from get user in active directory.
    /// </summary>
    interface IGetUserFromAD
    {
        /// <summary>
        /// Get users from Active Directory.
        /// </summary>
        /// <returns>Active users list.</returns>
        public List<UserDto> ADGetUsers();
    }
}
