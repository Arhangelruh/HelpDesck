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
        /// Lock user.
        /// </summary>
        /// <param name="profile">profileDto model</param>
        /// <returns>result from Lock</returns>
        Task Locking(int profileId);

        /// <summary>
        /// Relock user.
        /// </summary>
        /// <param name="profile">profileDto model</param>
        /// <returns>result</returns>
        Task UnLock(int profileId);

        /// <summary>
        /// Get profiles.
        /// </summary>      
        /// <returns>list userDto</returns>
        Task<IEnumerable<ProfileDto>> GetAsyncProfiles();

        /// <summary>
        /// Get profile.
        /// </summary>      
        /// <returns>ProfileDto</returns>
        Task<ProfileDto> GetProfileByIdAsync(int id);

        /// <summary>
        /// Delete user.
        /// </summary>      
        /// <returns>return result</returns>
        Task DeleteUserAsync(int id);
    }
}