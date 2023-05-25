using HelpDesk.BLL.Models;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Service from get user in active directory.
    /// </summary>
    public interface IGetUserFromAD
    {
        /// <summary>
        /// Get users from Active Directory.
        /// </summary>
        /// <returns>Active users list.</returns>
        Task<List<UserDto>> ADGetUsers();

        /// <summary>
        /// Build Sid number from binary data
        /// </summary>
        /// <param name="sid"></param>
        /// <returns>string SID</returns>
        Task<string> BuildOctetString(SecurityIdentifier sid);
    }
}
