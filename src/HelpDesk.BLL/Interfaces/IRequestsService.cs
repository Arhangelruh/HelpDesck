using HelpDesk.BLL.Models;
using HelpDesk.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.BLL.Interfaces
{
    /// <summary>
    /// Class from works whith requests.
    /// </summary>
    public interface IRequestsService
    {
        /// <summary>
        /// Add request to base.
        /// </summary>
        /// <param name="request">Dto model</param>
        Task AddRequestAsync(RequestDto request);

        /// <summary>
        /// Get all request.
        /// </summary>
        /// <returns>List Requests</returns>       
        Task  <List<RequestDto>> GetAllRequestsAsync();

        /// <summary>
        /// Get all request by user.
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <returns>List Requests from user creator</returns>
        Task<List<RequestDto>> GetRequestsByUserAsync(int profileId);

        /// <summary>
        /// Get request by id.
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns>Request model</returns>
        Task<RequestDto> GetRequestByIdAsync(int id);

        /// <summary>
        /// Change status from request.
        /// </summary>
        /// <param name="request">Request dto</param>
        /// <param name="statusId">Status id</param>
        Task ChangeStatusAsync(RequestDto request, int statusId);

        /// <summary>
        /// Add request to base.
        /// </summary>
        /// <param name="request">Dto model</param>
        Task DeleteRequestAsync(RequestDto request);

        /// <summary>
        /// Get user creater requst and admin who support request.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns>RequestDto fields admin and user id</returns>
        Task<RequestDto> GetUserAndAdminProblemAsync(Problem problem);

        /// <summary>
        /// Edit request in base.
        /// </summary>
        /// <param name="request">Dto model</param>
        Task EditRequestAsync(RequestDto request);
    }
}
