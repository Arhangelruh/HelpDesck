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
        /// Add request status.
        /// </summary>
        /// <param name="statusDbo"></param>        
        Task AddStatusAsync(StatusDto statusDbo);

        /// <summary>
        /// Get all statuses.
        /// </summary>
        /// <returns> list statusDbo models</returns>
        Task<List<StatusDto>> GetStatusesAsync();

        /// <summary>
        /// Edit status.
        /// </summary>
        /// <param name="id"></param>        
        Task EditStatusAsync(StatusDto status);

        /// <summary>
        /// Delete status.
        /// </summary>        
        Task DeleteStatusAsync(StatusDto statusDto);

        /// <summary>
        /// Get status by queue.
        /// </summary>
        /// <param name="queue"></param>        
        Task<StatusDto> SearchStatusAsync(int queue);

    }
}
