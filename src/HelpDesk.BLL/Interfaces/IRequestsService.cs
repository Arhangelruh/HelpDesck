using HelpDesk.BLL.Models;
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
        Task AddRequest(RequestDto request);

        /// <summary>
        /// Get all request.
        /// </summary>
        /// <returns>List Requests</returns>       
        Task  <List<RequestDto>> GetAllRequests();

        /// <summary>
        /// Get all request by user.
        /// </summary>
        /// <param name="profileId">Profile id</param>
        /// <returns>List Requests from user creator</returns>
        Task<List<RequestDto>> GetRequestsByUser(int profileId);

        /// <summary>
        /// Get request by id.
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns>Request model</returns>
        Task<RequestDto> GetRequestById(int id);

        /// <summary>
        /// Change status from request.
        /// </summary>
        /// <param name="request">Request dto</param>
        /// <param name="statusId">Status id</param>
        Task ChangeStatus(RequestDto request, int statusId);

        /// <summary>
        /// Add request to base.
        /// </summary>
        /// <param name="request">Dto model</param>
        Task DeleteRequest(RequestDto request);
    }
}
