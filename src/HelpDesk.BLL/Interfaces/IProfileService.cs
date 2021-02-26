using HelpDesk.BLL.Models;
using System.Collections.Generic;

namespace HelpDesk.BLL.Interfaces
{
    public interface IProfileService
    {
        /// <summary>
        /// Get users from Active Directory.
        /// </summary>
        /// <returns></returns>
        public List<UserDto> ADGetUsers();
    }
}