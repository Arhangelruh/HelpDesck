using HelpDesk.BLL.Models;
using HelpDesk.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Service from controle user profile.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Add users, users role, and profile from directory. 
        /// </summary>
        /// <param name="List<UserDto>">List dto models</param>
        Task AddAsyncUsers(List<UserDto> users);

        /// <summary>
        /// Add user.
        /// </summary>
        /// <param name="user">userdto model</param>
        /// <returns>result from add user to database</returns>
        Task AddUserAsync(UserDto user);

        /// <summary>
        /// Add profile.
        /// </summary>
        /// <param name="user">profiledto model</param>
        /// <returns>result from add profile to database</returns>
        Task AddProfileAsync(ProfileDto profile);

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="user">userDto model</param>
        /// <returns>result from changes users</returns>
        Task EditUserAsync(UserDto user);

        /// <summary>
        /// Search user by sid.
        /// </summary>
        /// <param name="sid">user AD sid</param>
        /// <returns>user model</returns>
        Task<User> GetUserBySid(string sid);

        /// <summary>
        /// Search profile by iser id.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>profileDto model</returns>
        Task<ProfileDto> GetProfileByUserId(string userId);

        /// <summary>
        /// Edit user profile.
        /// </summary>
        /// <param name="profile">profileDto model</param>
        /// <returns>result from changes</returns>
        Task EditProfile(ProfileDto profile);

        /// <summary>
        /// Lock user
        /// </summary>
        /// <param name="profile">profileDto model</param>
        /// <returns>result from Lock</returns>
        Task Locking(ProfileDto profile);

        /// <summary>
        /// Relock user
        /// </summary>
        /// <param name="profile">profileDto model</param>
        /// <returns>result</returns>
        Task ReLock(ProfileDto profile);

        /// <summary>
        /// Get profiles
        /// </summary>      
        /// <returns>list userDto</returns>
        Task<IEnumerable<ProfileDto>> GetAsyncProfiles();
    }
}